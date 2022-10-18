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
                new LabWorkflowEventDependencyException(
                    failedLabWorkflowEventDependencyException);

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
                broker.EnqueueLabWorkflowEventMessageAsync(It.IsAny<Message>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLabWorkflowEventDependencyException))),
                        Times.Once);

            this.queueBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MessageQueueDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddAndLogItAsync(Exception exception)
        {
            // given
            LabWorkflow randomLabWorkflow = CreateRandomLabWorkflow();

            var failedLabWorkflowEventDependencyException =
                new FailedLabWorkflowEventDependencyException(exception);

            var expectedLabWorkflowEventDependencyException =
                new LabWorkflowEventDependencyException(
                    failedLabWorkflowEventDependencyException);

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
                broker.EnqueueLabWorkflowEventMessageAsync(It.IsAny<Message>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowEventDependencyException))),
                        Times.Once);

            this.queueBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfArgumentExceptionOccursAndLogItAsync()
        {
            // given
            LabWorkflow randomLabWorkflow = CreateRandomLabWorkflow();
            var argumentException = new ArgumentException();

            var invalidLabWorkflowEventException =
                new InvalidLabWorkflowEventException(argumentException);

            var expectedLabWorkflowEventDependencyValidationException =
                new LabWorkflowEventDependencyValidationException(
                    invalidLabWorkflowEventException);

            this.queueBrokerMock.Setup(broker =>
                broker.EnqueueLabWorkflowEventMessageAsync(It.IsAny<Message>()))
                    .Throws(argumentException);

            // when
            ValueTask<LabWorkflow> addLabWorkflowEventTask =
                this.labWorkflowEventService.AddLabWorkflowEventAsync(randomLabWorkflow);

            LabWorkflowEventDependencyValidationException actualLabWorkflowEventDependencyValidationException =
                await Assert.ThrowsAsync<LabWorkflowEventDependencyValidationException>(
                    addLabWorkflowEventTask.AsTask);

            // then
            actualLabWorkflowEventDependencyValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowEventDependencyValidationException);

            this.queueBrokerMock.Verify(broker =>
                broker.EnqueueLabWorkflowEventMessageAsync(It.IsAny<Message>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowEventDependencyValidationException))),
                        Times.Once);

            this.queueBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionIfServiceErrorOccursAndLogItAsync()
        {
            // given
            LabWorkflow someLabWorkflow = CreateRandomLabWorkflow();
            var exception = new Exception();

            var failedLabWorkflowEventServiceException =
                new FailedLabWorkflowEventServiceException(exception);

            var expectedLabWorkflowEventServiceException =
                new LabWorkflowEventServiceException(failedLabWorkflowEventServiceException);

            this.queueBrokerMock.Setup(broker =>
                broker.EnqueueLabWorkflowEventMessageAsync(It.IsAny<Message>()))
                    .Throws(exception);

            // when
            ValueTask<LabWorkflow> addLabWorkflowTask =
                this.labWorkflowEventService.AddLabWorkflowEventAsync(someLabWorkflow);

            LabWorkflowEventServiceException actualLabWorkflowEventServiceException =
                await Assert.ThrowsAsync<LabWorkflowEventServiceException>(
                    addLabWorkflowTask.AsTask);

            // then
            actualLabWorkflowEventServiceException.Should().BeEquivalentTo(
                expectedLabWorkflowEventServiceException);

            this.queueBrokerMock.Verify(broker =>
                broker.EnqueueLabWorkflowEventMessageAsync(It.IsAny<Message>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowEventServiceException))),
                        Times.Once);

            this.queueBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
