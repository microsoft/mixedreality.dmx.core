// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
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
        [MemberData(nameof(AllDependencyExceptions))]
        public async Task ShouldThrowOrchestrationDependencyExceptionOnRetrieveIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedLabOrchestrationDependencyException =
                new LabOrchestrationDependencyException(
                    dependencyException.InnerException as Xeption);

            this.labServiceMock.Setup(service =>
                service.RetrieveAllLabsWithDevices())
                    .Throws(dependencyException);

            // when
            ValueTask<List<Lab>> retrieveAllLabsTask =
                this.labOrchestrationService.RetrieveAllLabsAsync();

            LabOrchestrationDependencyException actualLabOrchestrationDependencyException =
                await Assert.ThrowsAsync<LabOrchestrationDependencyException>(
                    retrieveAllLabsTask.AsTask);

            // then
            actualLabOrchestrationDependencyException
                .Should().BeEquivalentTo(
                    expectedLabOrchestrationDependencyException);

            this.labServiceMock.Verify(service =>
                service.RetrieveAllLabsWithDevices(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabOrchestrationDependencyException))),
                        Times.Once);

            this.externalLabServiceMock.Verify(service =>
                service.RetrieveAllExternalLabsAsync(),
                    Times.Never);

            this.labServiceMock.VerifyNoOtherCalls();
            this.externalLabServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOrchestrationServiceExceptionOnRetrieveIfErrorOccursAndLogItAsync()
        {
            // given
            string randomMessage = GetRandomString();
            var exception = new Exception(randomMessage);

            var failedLabOrchestrationServiceException =
                new FailedLabOrchestrationServiceException(exception);

            var expectedLabOrchestrationServiceException =
                new LabOrchestrationServiceException(failedLabOrchestrationServiceException);

            this.labServiceMock.Setup(service =>
                service.RetrieveAllLabsWithDevices())
                    .Throws(exception);

            // when
            ValueTask<List<Lab>> retrieveLabsTask =
                this.labOrchestrationService.RetrieveAllLabsAsync();

            LabOrchestrationServiceException actualLabOrchestrationServiceException =
                await Assert.ThrowsAsync<LabOrchestrationServiceException>(
                    retrieveLabsTask.AsTask);

            // then
            actualLabOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedLabOrchestrationServiceException);

            this.labServiceMock.Verify(service =>
                service.RetrieveAllLabsWithDevices(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabOrchestrationServiceException))));

            this.externalLabServiceMock.Verify(service =>
                service.RetrieveAllExternalLabsAsync(),
                    Times.Never);

            this.labServiceMock.VerifyNoOtherCalls();
            this.externalLabServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
