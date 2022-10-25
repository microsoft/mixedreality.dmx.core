// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;
using DMX.Core.Api.Services.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Services.Foundations.LabWorkflowEvents;
using DMX.Core.Api.Services.Foundations.LabWorkflows;
using DMX.Core.Api.Services.Orchestrations.LabWorkflows;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

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

        public static TheoryData<Xeption> LabWorkflowDependencyValidationExceptions()
        {
            string randomErrorMessage = GetRandomString();
            var innerException = new Xeption(randomErrorMessage);

            return new TheoryData<Xeption>
            {
                new LabWorkflowValidationException(innerException),
                new LabWorkflowDependencyValidationException(innerException)
            };
        }

        public static TheoryData<Xeption> LabWorkflowDependencyExceptions()
        {
            string randomErrorMessage = GetRandomString();
            var innerException = new Xeption(randomErrorMessage);

            return new TheoryData<Xeption>
            {
                new LabWorkflowDependencyException(innerException),
                new LabWorkflowServiceException(innerException),
            };
        }

        private Expression<Func<Exception, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static LabWorkflow CreateRandomLabWorkflow() =>
            CreateLabWorkflowFiller(GetRandomDateTimeOffset()).Create();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static Filler<LabWorkflow> CreateLabWorkflowFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<LabWorkflow>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset);

            return filler;
        }
    }
}
