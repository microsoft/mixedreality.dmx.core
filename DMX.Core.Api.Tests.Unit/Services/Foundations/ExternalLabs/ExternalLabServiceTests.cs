// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using DMX.Core.Api.Brokers.LabApis;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Models.Foundations.ExternalLabs;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Services.Foundations.ExternalLabs;
using KellermanSoftware.CompareNetObjects;
using Moq;
using RESTFulSense.Exceptions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.ExternalLabs
{
    public partial class ExternalLabServiceTest
    {
        private readonly Mock<IExternalLabApiBroker> externalLabApiBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IExternalLabService externalLabService;

        public ExternalLabServiceTest()
        {
            this.externalLabApiBrokerMock = new Mock<IExternalLabApiBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.externalLabService = new ExternalLabService(
                externalLabApiBroker: this.externalLabApiBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        public static TheoryData CriticalDependencyException()
        {
            string someMessage = GetRandomString();
            var someResponseMessage = new HttpResponseMessage();

            return new TheoryData<Xeption>()
            {
                new HttpResponseUrlNotFoundException(someResponseMessage, someMessage),
                new HttpResponseUnauthorizedException(someResponseMessage, someMessage),
                new HttpResponseForbiddenException(someResponseMessage, someMessage)
            };
        }

        private Expression<Func<ExternalLabServiceInformation, bool>> SameInformationAs(
            ExternalLabServiceInformation expectedExternalLabServiceInformation)
        {
            return actualExternalLabServiceInformation =>
                this.compareLogic.Compare(
                    expectedExternalLabServiceInformation,
                    actualExternalLabServiceInformation)
                        .AreEqual;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static List<dynamic> CreateRandomLabsProperties()
        {
            int randomCount = GetRandomNumber();

            (IDictionary<string, string> randomProperties, List<LabDevice> randomDevices) =
                GetRandomLabProperties();

            var allCases = new List<dynamic>
            {
                new
                {
                    Id = GetRandomString(),
                    Name = GetRandomString(),
                    IsConnected = true,
                    IsReserved = false,
                    LabStatus = LabStatus.Available,
                    Properties = randomProperties,
                    Devices = randomDevices
                },

                new
                {
                    Id = GetRandomString(),
                    Name = GetRandomString(),
                    IsConnected = true,
                    IsReserved = true,
                    LabStatus = LabStatus.Reserved,
                    Properties = randomProperties,
                    Devices = randomDevices
                },

                new
                {
                    Id = GetRandomString(),
                    Name = GetRandomString(),
                    IsConnected = false,
                    IsReserved = GetRandomBoolean(),
                    LabStatus = LabStatus.Offline,
                    Properties = randomProperties,
                    Devices = randomDevices
                }
            };

            return Enumerable.Range(start: 0, count: randomCount)
                .Select(iterator => allCases)
                    .SelectMany(@case => @case)
                        .ToList();
        }

        private static bool GetRandomBoolean() => new Random().Next(2) == 1;

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomPowerLevel() =>
            new IntRange(min: 0, max: 101).GetValue();

        private static ExternalLabCollection CreateRandomLabCollection() =>
            CreateExternalLabCollectionFiller().Create();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Filler<ExternalLabCollection> CreateExternalLabCollectionFiller()
        {
            var filler = new Filler<ExternalLabCollection>();

            filler.Setup()
                .OnType<DateTimeOffset?>().Use(GetRandomDateTimeOffset());

            return filler;
        }

        private static (IDictionary<string, string>, List<LabDevice>) GetRandomLabProperties()
        {
            string randomPhoneName = GetRandomString();
            string randomHMDName = GetRandomString();
            bool randomHostConnectionStatus = GetRandomBoolean();
            bool randomPhoneConnectionStatus = GetRandomBoolean();
            bool randomHMDConnectionStatus = GetRandomBoolean();
            int randomPhonePowerLevel = GetRandomPowerLevel();
            int randomHMDPowerLevel = GetRandomPowerLevel();

            var externalDeviceProperties = new Dictionary<string, string>
            {
                { @"Host\isconnected", $"{randomHostConnectionStatus}" },
                { @"Phone\name", randomPhoneName },
                { @"Phone\isconnected", $"{randomPhoneConnectionStatus}" },
                { @"Phone\powerlevel", $"{randomPhonePowerLevel}" },
                { @"HMD\name", randomHMDName },
                { @"HMD\isconnected", $"{randomHMDConnectionStatus}" },
                { @"HMD\powerlevel", $"{randomHMDPowerLevel}" },
            };

            var labDevices = new List<LabDevice>
            {
                new LabDevice
                {
                    Name = null,
                    Type = LabDeviceType.PC,
                    Category = LabDeviceCategory.Host,
                    PowerLevel = null,

                    Status = randomHostConnectionStatus
                        ? LabDeviceStatus.Online
                        : LabDeviceStatus.Offline,
                },

                new LabDevice
                {
                    Name = randomPhoneName,
                    Type = LabDeviceType.Phone,
                    Category = LabDeviceCategory.Attachment,
                    PowerLevel = randomPhonePowerLevel,

                    Status = randomPhoneConnectionStatus
                        ? LabDeviceStatus.Online
                        : LabDeviceStatus.Offline,
                },

                new LabDevice
                {
                    Name = randomHMDName,
                    Type = LabDeviceType.HeadMountedDisplay,
                    Category = LabDeviceCategory.Attachment,
                    PowerLevel = randomHMDPowerLevel,

                    Status = randomHMDConnectionStatus
                        ? LabDeviceStatus.Online
                        : LabDeviceStatus.Offline,
                },
            };

            return (externalDeviceProperties, labDevices);
        }
    }
}
