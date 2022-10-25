// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Orchestrations.LabWorkflows.Exceptions;
using FluentAssertions;
using Moq;
using Xeptions;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations.LabWorkflows
{
    public partial class LabWorkflowOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(LabWorkflowDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            LabWorkflow randomLabWorkflow = CreateRandomLabWorkflow();
            LabWorkflow someLabWorkflow = randomLabWorkflow;

            var expectedLabWorkflowOrchestrationDependencyValidationException =
                new LabWorkflowOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.labWorkflowServiceMock.Setup(service =>
                service.AddLabWorkflowAsync(It.IsAny<LabWorkflow>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<LabWorkflow> actualSubmitLabWorklowTask =
                this.labWorkflowOrchestrationService.SubmitLabWorkflowAsync(
                    someLabWorkflow);

            LabWorkflowOrchestrationDependencyValidationException
                actualLabWorkflowOrchestrationDependencyValidationException =
                    await Assert.ThrowsAsync<LabWorkflowOrchestrationDependencyValidationException>(
                        actualSubmitLabWorklowTask.AsTask);

            // then
            actualLabWorkflowOrchestrationDependencyValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowOrchestrationDependencyValidationException);

            this.labWorkflowServiceMock.Verify(service =>
                service.AddLabWorkflowAsync(It.IsAny<LabWorkflow>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowOrchestrationDependencyValidationException))),
                        Times.Once);

            this.labWorkflowCommandServiceMock.Verify(service =>
                service.AddLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.labWorkflowEventServiceMock.Verify(service =>
                service.AddLabWorkflowEventAsync(It.IsAny<LabWorkflow>()),
                    Times.Never);

            this.labWorkflowServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.labWorkflowCommandServiceMock.VerifyNoOtherCalls();
            this.labWorkflowEventServiceMock.VerifyNoOtherCalls();
        }
    }
}