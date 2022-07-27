using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Orchestrations.Labs.Exceptions;
using FluentAssertions;
using Moq;
using Xeptions;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations
{
    public partial class LabOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(LabDependencyValidationExceptions))]
        public async Task ShouldThrowOrchestrationDependencyValidationExceptionOnRetrieveByIdIfDependencyValidationErrorOccursAndLogItAsync(
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
            ValueTask<Lab> actualLabTask =
                this.labOrchestrationService.RetrieveLabByIdAsync(someLabId);

            LabOrchestrationDependencyValidationException
                actualLabOrchestrationDependencyValidationException =
                    await Assert.ThrowsAsync<LabOrchestrationDependencyValidationException>(
                        actualLabTask.AsTask);

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
        public async Task ShouldThrowOrchestrationDependencyExceptionOnRetrieveByIdDependencyErrorOccursAndLogItAsync(
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
            ValueTask<Lab> retrieveLabTask =
                this.labOrchestrationService.RetrieveLabByIdAsync(someLabId);

            LabOrchestrationDependencyException actualLabOrchestrationDependencyException =
                await Assert.ThrowsAsync<LabOrchestrationDependencyException>(
                    retrieveLabTask.AsTask);

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
    }
}
