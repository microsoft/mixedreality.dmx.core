// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Orchestrations.Labs.Exceptions;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xeptions;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations
{
    public partial class LabOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(LabDependencyValidationExceptions))]
        public async Task ShouldThrowOrchestrationDependencyValidationExceptionOnAdIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption validationException)
        {
            // given
            Lab randomLab = CreateRandomLab();
            Lab someLab = randomLab;

            var expectedLabOrchestrationDependencyValidationException =
                new LabOrchestrationDependencyValidationException(
                    validationException.InnerException as Xeption);

            this.labServiceMock.Setup(service =>
                service.AddLabAsync(It.IsAny<Lab>()))
                    .ThrowsAsync(validationException);

            // when
            ValueTask<Lab> actualLabTask =
                this.labOrchestrationService.AddLabAsync(someLab);

            LabOrchestrationDependencyValidationException
                actualLabOrchestrationDependencyValidationException =
                    await Assert.ThrowsAsync<LabOrchestrationDependencyValidationException>(
                        actualLabTask.AsTask);

            // then
            actualLabOrchestrationDependencyValidationException.Should()
                .BeEquivalentTo(expectedLabOrchestrationDependencyValidationException);

            this.labServiceMock.Verify(service =>
                service.AddLabAsync(It.IsAny<Lab>()),
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
        public async Task ShouldThrowOrchestrationDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Lab randomLab = CreateRandomLab();
            Lab inputLab = randomLab.DeepClone();

            var expectedLabOrchestrationDependencyException =
                new LabOrchestrationDependencyException(
                    dependencyException.InnerException as Xeption);

            this.labServiceMock.Setup(service =>
                service.AddLabAsync(inputLab))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<Lab> addLabTask =
                this.labOrchestrationService.AddLabAsync(inputLab);

            LabOrchestrationDependencyException actualLabOrchestrationDependencyException =
                await Assert.ThrowsAsync<LabOrchestrationDependencyException>(
                    addLabTask.AsTask);

            // then
            actualLabOrchestrationDependencyException
                .Should().BeEquivalentTo(
                    expectedLabOrchestrationDependencyException);

            this.labServiceMock.Verify(service =>
                service.AddLabAsync(It.IsAny<Lab>()),
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
        public async Task ShouldThrowOrchestrationServiceExceptionOnAddIfErrorOccursAndLogItAsync()
        {
            // given
            Lab randomLab = CreateRandomLab();
            Lab inputLab = randomLab;
            string randomMessage = GetRandomString();
            var exception = new Exception(randomMessage);

            var failedLabOrchestrationServiceException =
                new FailedLabOrchestrationServiceException(exception); 

            var expectedLabOrchestrationServiceException =
                new LabOrchestrationServiceException(failedLabOrchestrationServiceException);

            this.labServiceMock.Setup(service =>
                service.AddLabAsync(inputLab))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Lab> actualLabTask =
                this.labOrchestrationService.AddLabAsync(inputLab);

            LabOrchestrationServiceException actualLabOrchestrationServiceException =
                await Assert.ThrowsAsync<LabOrchestrationServiceException>(
                    actualLabTask.AsTask);

            // then
            actualLabOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedLabOrchestrationServiceException);

            this.labServiceMock.Verify(service =>
                service.AddLabAsync(It.IsAny<Lab>()),
                    Times.Once());

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
