// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions;
using DMX.Core.Api.Models.Foundations.LabCommands;
using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Moq;
using Xeptions;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabCommandEvents
{
    public partial class LabCommandEventServiceTests
    {
        [Theory]
        [MemberData(nameof(MessageQueueExceptions))]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync(
            Exception messageQueueException)
        {
            // given
            LabCommand someLabCommand = CreateRandomLabCommand();

            var failedLabCommandEventDependencyException =
                new FailedLabCommandEventDependencyException(messageQueueException);

            var expectedLabCommandEventDependencyException =
                new LabCommandEventDependencyException(
                    failedLabCommandEventDependencyException);

            this.queueBrokerMock.Setup(broker =>
                broker.EnqueueLabCommandEventMessageAsync(It.IsAny<Message>()))
                    .Throws(messageQueueException);

            // when
            ValueTask<LabCommand> addLabCommandTask =
                this.labCommandEventService.AddLabCommandEventAsync(someLabCommand);

            LabCommandEventDependencyException actualLabCommandEventDependencyException =
                await Assert.ThrowsAsync<LabCommandEventDependencyException>(
                    addLabCommandTask.AsTask);

            // then
            actualLabCommandEventDependencyException.Should().BeEquivalentTo(
                expectedLabCommandEventDependencyException);

            this.queueBrokerMock.Verify(broker =>
                broker.EnqueueLabCommandEventMessageAsync(It.IsAny<Message>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLabCommandEventDependencyException))),
                        Times.Once);

            this.queueBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MessageQueueDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync(
            Exception messageQueueDependencyException)
        { 
            // given
            LabCommand someLabCommand = CreateRandomLabCommand();

            var failedLabCommandEventDependencyException =
                new FailedLabCommandEventDependencyException(messageQueueDependencyException);

            var expectedLabCommandEventDepdendencyException = 
                new LabCommandEventDependencyException(failedLabCommandEventDependencyException);

            this.queueBrokerMock.Setup(broker =>
                broker.EnqueueLabCommandEventMessageAsync(It.IsAny<Message>()))
                    .Throws(messageQueueDependencyException);

            // when
            ValueTask<LabCommand> addLabCommandTask =
                this.labCommandEventService.AddLabCommandEventAsync(someLabCommand);

            LabCommandEventDependencyException actualLabCommandEventDepdendencyException =
                await Assert.ThrowsAsync<LabCommandEventDependencyException>(
                    addLabCommandTask.AsTask);

            // then
            actualLabCommandEventDepdendencyException.Should().BeEquivalentTo(
                expectedLabCommandEventDepdendencyException);

            this.queueBrokerMock.Verify(broker =>
                broker.EnqueueLabCommandEventMessageAsync(It.IsAny<Message>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandEventDepdendencyException))),
                        Times.Once);

            this.queueBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfArgumentExceptionOccursAndLogItAsync()
        {
            // given
            LabCommand someLabCommand = CreateRandomLabCommand();
            var argumentException = new ArgumentException();

            var invalidLabCommandEventArgumentException =
                new InvalidLabCommandEventArgumentException(argumentException);

            var expectedLabCommandEventDependencyValidationException =
                new LabCommandEventDependencyValidationException(
                    invalidLabCommandEventArgumentException);

            this.queueBrokerMock.Setup(broker =>
                broker.EnqueueLabCommandEventMessageAsync(It.IsAny<Message>()))
                    .Throws(argumentException);

            // when
            ValueTask<LabCommand> addLabCommandTask =
                this.labCommandEventService.AddLabCommandEventAsync(someLabCommand);

            LabCommandEventDependencyValidationException 
                actualLabCommandEventDependencyValidationException =
                    await Assert.ThrowsAsync<LabCommandEventDependencyValidationException>(
                        addLabCommandTask.AsTask);

            // then
            actualLabCommandEventDependencyValidationException.Should().BeEquivalentTo(
                expectedLabCommandEventDependencyValidationException);

            this.queueBrokerMock.Verify(broker =>
                broker.EnqueueLabCommandEventMessageAsync(It.IsAny<Message>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandEventDependencyValidationException))),
                        Times.Once);

            this.queueBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
