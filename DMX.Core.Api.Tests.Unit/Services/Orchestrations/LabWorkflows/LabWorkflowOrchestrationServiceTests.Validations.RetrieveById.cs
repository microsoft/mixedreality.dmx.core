// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;
using DMX.Core.Api.Models.Orchestrations.LabWorkflows;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
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

            var invalidLabWorkflowException =
                new InvalidLabWorkflowException();

            var expectedLabWorkflowOrchestrationValidationException =
                new LabWorkflowOrchestrationValidationException(
                    invalidLabWorkflowException);

            this.labWorkflowServiceMock.Setup(service =>
                service.RetrieveLabWorkflowByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(invalidLabWorkflowException);

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

            this.labWorkflowServiceMock.Verify(service =>
                service.RetrieveLabWorkflowByIdAsync(
                    It.IsAny<Guid>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowOrchestrationValidationException))),
                        Times.Once);

            this.labWorkflowServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.labWorkflowEventServiceMock.VerifyNoOtherCalls();
            this.labWorkflowCommandServiceMock.VerifyNoOtherCalls();
        }
    }
}
