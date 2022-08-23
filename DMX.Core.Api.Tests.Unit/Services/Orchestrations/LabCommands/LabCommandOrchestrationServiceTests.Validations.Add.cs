// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Orchestrations.LabCommands.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations.LabCommands
{
    public partial class LabCommandOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowOrchestrationValidationExceptionOnAddIfNullAndLogItAsync()
        {
            // given
            LabCommand nullLabCommand = null;
            LabCommand inputLabCommand = nullLabCommand;

            var nullLabCommandOrchestrationException =
                new NullLabCommandOrchestrationException();

            var expectedLabCommandOrchestrationValidationException =
                new LabCommandOrchestrationValidationException(
                    nullLabCommandOrchestrationException);

            // when
            ValueTask<LabCommand> addLabCommandTask =
                this.labCommandOrchestrationService.AddLabCommandAsync(inputLabCommand);

            LabCommandOrchestrationValidationException actualLabCommandOrchestrationValidationException =
                await Assert.ThrowsAsync<LabCommandOrchestrationValidationException>(
                    addLabCommandTask.AsTask);

            // then
            actualLabCommandOrchestrationValidationException
                .Should().BeEquivalentTo(
                    expectedLabCommandOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandOrchestrationValidationException))),
                        Times.Once);

            this.labCommandServiceMock.Verify(service =>
                service.AddLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.labCommandServiceMock.VerifyNoOtherCalls();
        }
    }
}
