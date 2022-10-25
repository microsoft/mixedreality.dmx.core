// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Orchestrations.LabWorkflows.Exceptions;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations.LabWorkflows
{
    public partial class LabWorkflowOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionIfLabWorkflowIsNullAndLogItAsync()
        {
            // given
            LabWorkflow nullLabWorkflow = null;

            var nullLabWorkflowOrchestrationException =
                new NullLabWorkflowOrchestrationException();

            var expectedLabWorkflowOrchestrationValidationException =
                new LabWorkflowOrchestrationValidationException(
                    nullLabWorkflowOrchestrationException);

            // when
            ValueTask<LabWorkflow> submitLabWorkflowTask =
                this.labWorkflowOrchestrationService
                    .SubmitLabWorkflowAsync(nullLabWorkflow);

            LabWorkflowOrchestrationValidationException actualLabWorkflowOrchestrationValidationException =
                await Assert.ThrowsAsync<LabWorkflowOrchestrationValidationException>(
                    submitLabWorkflowTask.AsTask);

            // then
            actualLabWorkflowOrchestrationValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowOrchestrationValidationException))),
                        Times.Once);

            this.labWorkflowServiceMock.Verify(service =>
                service.AddLabWorkflowAsync(It.IsAny<LabWorkflow>()),
                    Times.Never);

            this.labWorkflowCommandServiceMock.Verify(service =>
                service.AddLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.labWorkflowEventServiceMock.Verify(service =>
                service.AddLabWorkflowEventAsync(It.IsAny<LabWorkflow>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.labWorkflowServiceMock.VerifyNoOtherCalls();
            this.labWorkflowCommandServiceMock.VerifyNoOtherCalls();
            this.labWorkflowEventServiceMock.VerifyNoOtherCalls();
        }
    }
}
