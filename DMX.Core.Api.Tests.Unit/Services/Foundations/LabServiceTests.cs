// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.ReverbApis;
using DMX.Core.Api.Models.Externals.ExternalLabs;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Services.Foundations.Labs;
using KellermanSoftware.CompareNetObjects;
using Moq;
using RESTFulSense.Exceptions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations
{
    public partial class LabServiceTests
    {
        private readonly Mock<IReverbApiBroker> reverbApiBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly ILabService labService;

        public LabServiceTests()
        {
            this.reverbApiBrokerMock = new Mock<IReverbApiBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.labService = new LabService(
                reverbApiBroker: this.reverbApiBrokerMock.Object,
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

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message &&
                actualException.InnerException.Message == expectedException.InnerException.Message &&
                (actualException.InnerException as Xeption).DataEquals(expectedException.InnerException.Data);
        }

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

            Dictionary<string, string> externalDeviceProperties = new Dictionary<string, string>
            {
                { @"Host\isconnected", $"{randomHostConnectionStatus}" },
                { @"Phone\name", randomPhoneName },
                { @"Phone\isconnected", $"{randomPhoneConnectionStatus}" },
                { @"HMD\name", randomHMDName },
                { @"HMD\isconnected", $"{randomHMDConnectionStatus}" },
            };

            List<LabDevice> labDevices = new List<LabDevice>
            {
                new LabDevice
                {
                    Name = null,
                    Type = LabDeviceType.PC,
                    Category = LabDeviceCategory.Host,

                    Status = randomHostConnectionStatus
                        ? LabDeviceStatus.Online
                        : LabDeviceStatus.Offline,
                },

                new LabDevice
                {
                    Name = randomPhoneName,
                    Type = LabDeviceType.Phone,
                    Category = LabDeviceCategory.Attachment,

                    Status = randomPhoneConnectionStatus
                        ? LabDeviceStatus.Online
                        : LabDeviceStatus.Offline,
                },

                new LabDevice
                {
                    Name = randomHMDName,
                    Type = LabDeviceType.HeadMountedDisplay,
                    Category = LabDeviceCategory.Attachment,

                    Status = randomHMDConnectionStatus
                        ? LabDeviceStatus.Online
                        : LabDeviceStatus.Offline,
                },
            };

            return (externalDeviceProperties, labDevices);
        }
    }
}
