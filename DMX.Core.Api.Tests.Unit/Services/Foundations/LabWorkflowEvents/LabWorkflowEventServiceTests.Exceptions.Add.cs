// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowEvent.Exceptions;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflowEvents
{
    public partial class LabWorkflowEventServiceTests
    {
        [Theory]
        [MemberData(nameof(MessageQueueExceptions))]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddAndLogItAsync(Exception exception)
        {
            // given
            LabWorkflow randomLabWorkflow = CreateRandomLabWorkflow();

            var failedLabWorkflowEventDependencyException =
                new FailedLabWorkflowEventDependencyException(exception);

            var expectedLabWorkflowEventDependencyException =
                new LabWorkflowEventDependencyException(failedLabWorkflowEventDependencyException);

            this.queueBrokerMock.Setup(broker =>
                broker.EnqueueLabWorkflowEventMessageAsync(It.IsAny<Message>()))
                    .Throws(exception);

            // when
            ValueTask<LabWorkflow> addLabWorkflowEventTask =
                this.labWorkflowEventService.AddLabWorkflowEventAsync(randomLabWorkflow);

            LabWorkflowEventDependencyException actualLabWorkflowEventDependencyException =
                await Assert.ThrowsAsync<LabWorkflowEventDependencyException>(
                    addLabWorkflowEventTask.AsTask);

            // then
            actualLabWorkflowEventDependencyException.Should().BeEquivalentTo(
                expectedLabWorkflowEventDependencyException);

            this.queueBrokerMock.Verify(broker =>
                broker.EnqueueLabCommandEventMessageAsync(It.IsAny<Message>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLabWorkflowEventDependencyException))),
                        Times.Once);

            this.queueBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
