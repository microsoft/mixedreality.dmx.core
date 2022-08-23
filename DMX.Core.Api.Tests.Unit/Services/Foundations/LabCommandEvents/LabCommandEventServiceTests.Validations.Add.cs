// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabCommandEvents
{
    public partial class LabCommandEventServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfLabCommandEventIsNullAndLogItAsync()
        {
            // given
            LabCommand nullLabCommand = null;
            var nullLabCommandException = new NullLabCommandException();

            var expectedLabCommandEventValidationException =
                new LabCommandEventValidationException(nullLabCommandException);

            // when
            ValueTask<LabCommand> addLabCommandEventTask =
                this.labCommandEventService.AddLabCommandEventAsync(nullLabCommand);

            LabCommandEventValidationException actualLabCommandEventValidationException =
                await Assert.ThrowsAsync<LabCommandEventValidationException>(
                    addLabCommandEventTask.AsTask);

            // then
            actualLabCommandEventValidationException.Should().BeEquivalentTo(
                expectedLabCommandEventValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandEventValidationException))),
                        Times.Once);

            this.queueBrokerMock.Verify(broker =>
                broker.EnqueueLabCommandEventMessageAsync(It.IsAny<Message>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.queueBrokerMock.VerifyNoOtherCalls();
        }
    }
}
