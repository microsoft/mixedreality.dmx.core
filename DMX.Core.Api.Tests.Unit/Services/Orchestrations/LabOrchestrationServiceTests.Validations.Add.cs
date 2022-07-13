// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldThrowOrchestrationValidationExceptionOnAddIfNullAndLogItAsync()
        {
            // given
            Lab nullLab = null;
            Lab inputLab = nullLab;
            var nullLabException = new NullLabException();

            var expectedLabOrchestrationValidationException =
                new LabOrchestrationValidationException(nullLabException);

            // when
            ValueTask<Lab> addLabTask = this.labOrchestrationService.AddLabAsync(inputLab);

            LabOrchestrationValidationException actualLabOrchestrationValidationException =
                await Assert.ThrowsAsync<LabOrchestrationValidationException>(
                    addLabTask.AsTask);

            // then
            actualLabOrchestrationValidationException
                .Should().BeEquivalentTo(
                    expectedLabOrchestrationValidationException);

            this.labServiceMock.Verify(service =>
                service.AddLabAsync(It.IsAny<Lab>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabOrchestrationValidationException))),
                        Times.Once);

            this.labServiceMock.VerifyNoOtherCalls();
            this.externalLabServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
