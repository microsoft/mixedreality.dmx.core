// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabWorkflowEvent.Exceptions;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;
using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflowEvents
{
    public partial class LabWorkflowEventServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfLabWorkflowEventIsNullAndLogItAsync()
        {
            // given
            LabWorkflow nullLabWorkflow = null;

            var nullLabWorkflowException = new NullLabWorkflowException();

            var expectedLabWorkflowEventValidationException = 
                new LabWorkflowEventValidationException(nullLabWorkflowException);

            // when
            ValueTask<LabWorkflow> addLabWorkflowTask =
                this.labWorkflowEventService.AddLabWorkflowEventAsync(nullLabWorkflow);

            LabWorkflowEventValidationException actualLabWorkflowEventValidationException =
                await Assert.ThrowsAsync<LabWorkflowEventValidationException>(
                    addLabWorkflowTask.AsTask);

            // then
            actualLabWorkflowEventValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowEventValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowEventValidationException))),
                        Times.Once);

            this.queueBrokerMock.Verify(broker =>
                broker.EnqueueLabWorkflowEventMessageAsync(It.IsAny<Message>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.queueBrokerMock.VerifyNoOtherCalls();
        }
    }
}
