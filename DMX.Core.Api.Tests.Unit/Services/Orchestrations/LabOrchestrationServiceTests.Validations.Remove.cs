// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Foundations.Labs.Exceptions;
using DMX.Core.Api.Models.Orchestrations.Labs.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations
{
    public partial class LabOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowOrchestrationValidationExceptionOnAddIfLabIdIsInvalidAndLogItAsync()
        {
            // given
            var inputGuid = Guid.Empty;
            var invalidLabIdException =
                new InvalidLabIdException(inputGuid);

            var expectedLabOrchestrationValidationException =
                new LabOrchestrationValidationException(invalidLabIdException);

            // when
            ValueTask<Lab> actualLabTask =
                this.labOrchestrationService.RemoveLabByIdAsync(inputGuid);

            LabOrchestrationValidationException
                actualLabOrchestrationValidationException =
                    await Assert.ThrowsAsync<LabOrchestrationValidationException>(
                        actualLabTask.AsTask);

            // then
            actualLabOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedLabOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is( SameExceptionAs(
                    expectedLabOrchestrationValidationException))),
                        Times.Once);

            this.labServiceMock.Verify(service =>
                service.RemoveLabByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.labServiceMock.VerifyNoOtherCalls();
            this.externalLabServiceMock.VerifyNoOtherCalls();
        }
    }
}
