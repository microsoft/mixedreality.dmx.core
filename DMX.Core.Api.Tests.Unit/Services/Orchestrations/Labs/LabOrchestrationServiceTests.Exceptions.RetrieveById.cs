// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Orchestrations.Labs.Exceptions;
using FluentAssertions;
using Moq;
using Xeptions;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations.Labs
{
    public partial class LabOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(LabDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnRetrieveByIdIfDependencyValidationErrorOccurrsAndLogItAsync(
            Xeption validationException)
        {
            // given
            Guid someLabId = Guid.NewGuid();

            var expectedLabOrchestrationDependencyValidationException =
                new LabOrchestrationDependencyValidationException(
                    validationException.InnerException as Xeption);

            this.labServiceMock.Setup(service =>
                service.RetrieveLabByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ValueTask<Lab> retrieveLabByIdTask =
                this.labOrchestrationService.RetrieveLabByIdAsync(someLabId);

            LabOrchestrationDependencyValidationException
                actualLabOrchestrationDependencyValidationException =
                    await Assert.ThrowsAsync<LabOrchestrationDependencyValidationException>(
                        retrieveLabByIdTask.AsTask);

            // then
            actualLabOrchestrationDependencyValidationException.Should()
                .BeEquivalentTo(expectedLabOrchestrationDependencyValidationException);

            this.labServiceMock.Verify(service =>
                service.RetrieveLabByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabOrchestrationDependencyValidationException))),
                        Times.Once);

            this.labServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.externalLabServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(LabDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Guid someLabId = Guid.NewGuid();

            var expectedLabOrchestrationDependencyException =
                new LabOrchestrationDependencyException(
                    dependencyException.InnerException as Xeption);

            this.labServiceMock.Setup(service =>
                service.RetrieveLabByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<Lab> retrieveLabByIdTask =
                this.labOrchestrationService.RetrieveLabByIdAsync(someLabId);

            LabOrchestrationDependencyException actualLabOrchestrationDependencyException =
                await Assert.ThrowsAsync<LabOrchestrationDependencyException>(
                    retrieveLabByIdTask.AsTask);

            // then
            actualLabOrchestrationDependencyException.Should()
                .BeEquivalentTo(expectedLabOrchestrationDependencyException);

            this.labServiceMock.Verify(service =>
                service.RetrieveLabByIdAsync(someLabId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabOrchestrationDependencyException))),
                        Times.Once);

            this.labServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.externalLabServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someLabId = Guid.NewGuid();
            string randomMessage = GetRandomString();
            var exception = new Exception(randomMessage);

            var failedLabOrchestrationServiceException =
                new FailedLabOrchestrationServiceException(exception);

            var expectedLabOrchestrationServiceException =
                new LabOrchestrationServiceException(failedLabOrchestrationServiceException);

            this.labServiceMock.Setup(service =>
                service.RetrieveLabByIdAsync(someLabId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Lab> retrieveLabByIdTask =
                this.labOrchestrationService.RetrieveLabByIdAsync(someLabId);

            LabOrchestrationServiceException actualLabOrchestrationServiceException =
                await Assert.ThrowsAsync<LabOrchestrationServiceException>(
                    retrieveLabByIdTask.AsTask);

            // then
            actualLabOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedLabOrchestrationServiceException);

            this.labServiceMock.Verify(service =>
                service.RetrieveLabByIdAsync(someLabId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabOrchestrationServiceException))),
                        Times.Once);

            this.labServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.externalLabServiceMock.VerifyNoOtherCalls();
        }
    }
}
