// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Services.Foundations.LabCommands;
using DMX.Core.Api.Services.Orchestrations;
using DMX.Core.Api.Services.Orchestrations.LabCommands;
using Moq;
using Tynamix.ObjectFiller;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations.LabCommands
{
    public partial class LabCommandOrchestrationServiceTests
    {
        private readonly Mock<ILabCommandService> labCommandServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private ILabCommandOrchestrationService labCommandOrchestrationService;

        public LabCommandOrchestrationServiceTests()
        {
            this.labCommandServiceMock = new Mock<ILabCommandService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.labCommandOrchestrationService = new LabCommandOrchestrationService(
                labCommandService: this.labCommandServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static LabCommand CreateRandomLabCommand() =>
            CreateLabCommandFiller(GetRandomDateTimeOffset()).Create();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static LabCommand CreateRandomLabCommand(DateTimeOffset date) =>
            CreateLabCommandFiller(date).Create();

        private static Filler<LabCommand> CreateLabCommandFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<LabCommand>();

            filler.Setup()
                .OnType<DateTimeOffset>()
                    .Use(dateTimeOffset);

            return filler;
        }
    }
}
