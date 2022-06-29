// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Labs;
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
        [MemberData(nameof(ExternalDependencyExceptions))]
        public async Task ShouldThrowOrchestrationDependencyExceptionOnRetrieveIfExternalDependencyErrorOccursAndLogItAsync(
            Xeption externalDependencyException)
        {
            // given
            var expectedLabOrchestrationDependencyException =
                new LabOrchestrationDependencyException(
                    externalDependencyException.InnerException as Xeption);

            this.externalLabServiceMock.Setup(service =>
                service.RetrieveAllLabsAsync())
                    .ThrowsAsync(externalDependencyException);

            // when
            ValueTask<List<Lab>> retrieveAllLabsTask = this.labOrchestrationService.RetrieveAllLabsAsync();

            LabOrchestrationDependencyException actualLabOrchestrationDependencyException =
                await Assert.ThrowsAsync<LabOrchestrationDependencyException>(
                    retrieveAllLabsTask.AsTask);

            // then
            actualLabOrchestrationDependencyException
                .Should().BeEquivalentTo(
                    expectedLabOrchestrationDependencyException);

            this.externalLabServiceMock.Verify(service =>
                service.RetrieveAllLabsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabOrchestrationDependencyException))),
                        Times.Once);

            this.externalLabServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
