// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfNullAndLogItAsync()
        {
            // given
            LabWorkflowCommand nullLabWorkflowCommand = null;
            var nullLabWorkflowCommandException = new NullLabWorkflowCommandException();

            var expectedLabWorkflowCommandValidationException =
                new LabWorkflowCommandValidationException(nullLabWorkflowCommandException);

            // when
            ValueTask<LabWorkflowCommand> addLabWorkflowCommandTask =
                this.labWorkflowCommandService.AddLabWorkflowCommandAsync(nullLabWorkflowCommand);

            LabWorkflowCommandValidationException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandValidationException>(
                    addLabWorkflowCommandTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabWorkflowCommandAsync(
                    It.IsAny<LabWorkflowCommand>()),
                        Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
