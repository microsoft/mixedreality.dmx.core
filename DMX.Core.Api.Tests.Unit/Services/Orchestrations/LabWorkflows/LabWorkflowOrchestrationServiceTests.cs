// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Services.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Services.Foundations.LabWorkflowEvents;
using DMX.Core.Api.Services.Foundations.LabWorkflows;
using DMX.Core.Api.Services.Orchestrations.LabWorkflows;
using Moq;
using Tynamix.ObjectFiller;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations.LabWorkflows
{
    public partial class LabWorkflowOrchestrationServiceTests
    {
        private readonly Mock<ILabWorkflowService> labWorkflowServiceMock;
        private readonly Mock<ILabWorkflowCommandService> labWorkflowCommandServiceMock;
        private readonly Mock<ILabWorkflowEventService> labWorkflowEventServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private ILabWorkflowOrchestrationService labWorkflowOrchestrationService;

        public LabWorkflowOrchestrationServiceTests()
        {
            this.labWorkflowServiceMock = new Mock<ILabWorkflowService>();
            this.labWorkflowCommandServiceMock = new Mock<ILabWorkflowCommandService>();
            this.labWorkflowEventServiceMock = new Mock<ILabWorkflowEventService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.labWorkflowOrchestrationService = new LabWorkflowOrchestrationService(
                labWorkflowService: this.labWorkflowServiceMock.Object,
                labWorkflowCommandService: this.labWorkflowCommandServiceMock.Object,
                labWorkflowEventService: this.labWorkflowEventServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        public static LabWorkflow CreateRandomLabWorkflow() =>
            CreateLabWorkflowFiller(GetRandomDateTimeOffset()).Create();

        public static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Filler<LabWorkflow> CreateLabWorkflowFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<LabWorkflow>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset);

            return filler;
        }
    }
}
