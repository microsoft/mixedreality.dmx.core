﻿// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Storages;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Services.Foundations.Labs;
using Moq;
using Tynamix.ObjectFiller;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.Labs
{
    public partial class LabServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ILabService labService;

        public LabServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.labService = new LabService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }
        private static Lab CreateRandomLab() =>
            CreateLabFiller().Create();

        private static Filler<Lab> CreateLabFiller() =>
            new Filler<Lab>();
    }
}