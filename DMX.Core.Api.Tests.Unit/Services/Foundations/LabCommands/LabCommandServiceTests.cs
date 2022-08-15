// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Storages;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Services.Foundations.LabCommands;
using Moq;
using Tynamix.ObjectFiller;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabCommands
{
    public partial class LabCommandServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ILabCommandService labCommandService;

        public LabCommandServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.labCommandService = new LabCommandService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static LabCommand CreateRandomLabCommand() =>
            GetLabCommandFiller().Create();

        private static Filler<LabCommand> GetLabCommandFiller()
        {
            var filler = new Filler<LabCommand>();

            filler.Setup()
                .OnType<DateTimeOffset>()
                    .Use(GetRandomDateTimeOffset());

            return filler;
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}
