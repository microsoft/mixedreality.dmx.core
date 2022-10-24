// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Orchestrations.LabWorkflows;
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

        [Theory]
        [MemberData(nameof(LabWorkflowDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Guid someGuid = Guid.NewGuid();

            var expectedLabWorkflowOrchestrationDependencyException =
                new LabWorkflowOrchestrationDependencyException(
                    dependencyException.InnerException as Xeption);

            this.labWorkflowServiceMock.Setup(service =>
                service.RetrieveLabWorkflowByIdAsync(It.IsAny<Guid>()))
                        .ThrowsAsync(dependencyException);

            // when
            ValueTask<LabWorkflow> retrieveLabWorkflowTask =
                this.labWorkflowOrchestrationService.RetrieveLabWorkflowByIdAsync(someGuid);

            LabWorkflowOrchestrationDependencyException actualLabWorkflowOrchestrationDependencyException =
                await Assert.ThrowsAsync<LabWorkflowOrchestrationDependencyException>(
                    retrieveLabWorkflowTask.AsTask);

            // then
            actualLabWorkflowOrchestrationDependencyException.Should().BeEquivalentTo(
                expectedLabWorkflowOrchestrationDependencyException);

            this.labWorkflowServiceMock.Verify(service =>
                service.RetrieveLabWorkflowByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowOrchestrationDependencyException))),
                        Times.Once);

            this.labWorkflowServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.labWorkflowCommandServiceMock.VerifyNoOtherCalls();
            this.labWorkflowEventServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someLabId = Guid.NewGuid();
            string randomMessage = GetRandomString();
            var exception = new Exception(randomMessage);

            var failedLabWorkflowOrchestrationServiceException =
                new FailedLabWorkflowOrchestrationServiceException(exception);

            var expectedLabWorkflowOrchestrationServiceException =
                new LabWorkflowOrchestrationServiceException(
                    failedLabWorkflowOrchestrationServiceException);

            this.labWorkflowServiceMock.Setup(service =>
                service.RetrieveLabWorkflowByIdAsync(someLabId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<LabWorkflow> retrievedLabWorkflowByIdTask =
                this.labWorkflowOrchestrationService.RetrieveLabWorkflowByIdAsync(
                    someLabId);

            LabWorkflowOrchestrationServiceException actualLabWorkflowOrchestrationServiceException =
                await Assert.ThrowsAsync<LabWorkflowOrchestrationServiceException>(
                    retrievedLabWorkflowByIdTask.AsTask);

            // then
            actualLabWorkflowOrchestrationServiceException.Should().BeEquivalentTo(
                expectedLabWorkflowOrchestrationServiceException);

            this.labWorkflowServiceMock.Verify(service =>
                service.RetrieveLabWorkflowByIdAsync(
                    It.IsAny<Guid>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowOrchestrationServiceException))),
                        Times.Once);

            this.labWorkflowServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.labWorkflowCommandServiceMock.VerifyNoOtherCalls();
            this.labWorkflowEventServiceMock.VerifyNoOtherCalls();
        }
    }
}
