// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Orchestrations.LabWorkflows;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveByIdIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Guid someLabWorkflowGuid = Guid.NewGuid();

            var expectedLabWorkflowOrchestrationDependencyValidationException =
                new LabWorkflowOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.labWorkflowServiceMock.Setup(service =>
                service.RetrieveLabWorkflowByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<LabWorkflow> retrievedLabWorkflowByIdTask =
                this.labWorkflowOrchestrationService.RetrieveLabWorkflowByIdAsync(someLabWorkflowGuid);

            LabWorkflowOrchestrationDependencyValidationException
                actualLabWorkflowOrchestrationDependencyValidationException =
                    await Assert.ThrowsAsync<LabWorkflowOrchestrationDependencyValidationException>(
                        retrievedLabWorkflowByIdTask.AsTask);

            // then
            actualLabWorkflowOrchestrationDependencyValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowOrchestrationDependencyValidationException);

            this.labWorkflowServiceMock.Verify(service =>
                service.RetrieveLabWorkflowByIdAsync(
                    It.IsAny<Guid>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowOrchestrationDependencyValidationException))),
                        Times.Once);

            this.labWorkflowServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.labWorkflowCommandServiceMock.VerifyNoOtherCalls();
            this.labWorkflowEventServiceMock.VerifyNoOtherCalls();
        }
    }
}
