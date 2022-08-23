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
                new LabCommandEventDepdendencyException(
                    failedLabCommandEventDependencyException);

            this.queueBrokerMock.Setup(broker =>
                broker.EnqueueLabCommandEventMessageAsync(It.IsAny<Message>()))
                    .Throws(messageQueueException);

            // when
            ValueTask<LabCommand> addLabCommandTask =
                this.labCommandEventService.AddLabCommandEventAsync(someLabCommand);

            LabCommandEventDepdendencyException actualLabCommandEventDependencyException =
                await Assert.ThrowsAsync<LabCommandEventDepdendencyException>(
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
    }
}
