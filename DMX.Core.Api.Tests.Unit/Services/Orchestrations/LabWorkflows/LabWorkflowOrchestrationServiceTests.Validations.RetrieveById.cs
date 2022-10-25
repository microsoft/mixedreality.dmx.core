// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Orchestrations.LabWorkflows.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations.LabWorkflows
{
    public partial class LabWorkflowOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfLabWorkflowIsInvalidAndLogItAsync()
        {
            // given
            Guid emptyLabWorkflowId = Guid.Empty;

            var invalidLabWorkflowOrchestrationException =
                new InvalidLabWorkflowOrchestrationException();

            invalidLabWorkflowOrchestrationException.AddData(
                key: nameof(LabWorkflow.Id),
                values: "Id is required");

            var expectedLabWorkflowOrchestrationValidationException =
                new LabWorkflowOrchestrationValidationException(
                    invalidLabWorkflowOrchestrationException);

            // when
            ValueTask<LabWorkflow> retrieveLabWorkflowTask =
                this.labWorkflowOrchestrationService
                    .RetrieveLabWorkflowByIdAsync(emptyLabWorkflowId);

            LabWorkflowOrchestrationValidationException
                actualLabWorkflowOrchestrationValidationException =
                    await Assert.ThrowsAsync<LabWorkflowOrchestrationValidationException>(
                        retrieveLabWorkflowTask.AsTask);

            // then
            actualLabWorkflowOrchestrationValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowOrchestrationValidationException))),
                        Times.Once);

            this.labWorkflowServiceMock.Verify(service =>
                service.RetrieveLabWorkflowByIdAsync(
                    It.IsAny<Guid>()),
                        Times.Never);

            this.labWorkflowServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.labWorkflowEventServiceMock.VerifyNoOtherCalls();
            this.labWorkflowCommandServiceMock.VerifyNoOtherCalls();
        }
    }
}
