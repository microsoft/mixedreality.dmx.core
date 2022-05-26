// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.ReverbApis;
using DMX.Core.Api.Models.External.ExternalLabs;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Services.Foundations;
using KellermanSoftware.CompareNetObjects;
using Moq;
using Tynamix.ObjectFiller;

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

        private Expression<Func<ExternalLabsServiceInformation, bool>> SameInformationAs(
            ExternalLabsServiceInformation expectedExternalLabsServiceInformation)
        {
            return actualExternalLabsServiceInformation =>
                this.compareLogic.Compare(
                    expectedExternalLabsServiceInformation,
                    actualExternalLabsServiceInformation)
                        .AreEqual;
        }

        private static List<dynamic> CreateRandomLabsProperties()
        {
            int randomCount = GetRandomNumber();
            bool randomIsConnected = GetRandomBoolean();
            bool randomIsReserved = GetRandomBoolean();

            LabStatus randomLabStatus = (randomIsConnected, randomIsReserved) switch
            {
                (false, _) => LabStatus.Offline,
                (true, true) => LabStatus.Reserved,
                _ => LabStatus.Available
            };

            return Enumerable.Range(start: 0, count: randomCount)
                .Select(item =>
                    new
                    {
                        Id = GetRandomString(),
                        Name = GetRandomString(),
                        IsConnected = randomIsConnected,
                        IsReserved = randomIsReserved,
                        LabStatus = randomLabStatus
                    }).ToList<dynamic>();
        }

        private static bool GetRandomBoolean() => new Random().Next(2) == 1;

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static ExternalLabsCollection CreateRandomLabCollection() =>
            CreateExternalLabCollectionFiller().Create();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Filler<ExternalLabsCollection> CreateExternalLabCollectionFiller()
        {
            var filler = new Filler<ExternalLabsCollection>();

            filler.Setup()
                .OnType<DateTimeOffset?>().Use(GetRandomDateTimeOffset());

            return filler;
        }
    }
}
