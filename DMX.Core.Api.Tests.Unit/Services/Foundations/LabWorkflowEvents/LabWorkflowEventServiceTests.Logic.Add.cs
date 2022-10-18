// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using FluentAssertions;
using Force.DeepCloner;
using Microsoft.Azure.ServiceBus;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflowEvents
{
    public partial class LabWorkflowEventServiceTests
    {
        [Fact]
        public async Task ShouldAddLabWorkflowEventAsync()
        {
            // given
            LabWorkflow randomLabWorkflow = CreateRandomLabWorkflow();
            LabWorkflow inputLabWorkflow = randomLabWorkflow;
            LabWorkflow expectedLabWorkfow = randomLabWorkflow.DeepClone();

            string serializedLabWorkflow =
                JsonSerializer.Serialize(expectedLabWorkfow);

            var expectedLabWorkflowMessage = new Message
            {
                Body = Encoding.UTF8.GetBytes(serializedLabWorkflow)
            };

            // when
            LabWorkflow actualLabWorkflow =
                await this.labWorkflowEventService
                    .AddLabWorkflowEventAsync(inputLabWorkflow);

            // then
            actualLabWorkflow.Should().BeEquivalentTo(expectedLabWorkfow);

            this.queueBrokerMock.Verify(broker =>
                broker.EnqueueLabWorkflowEventMessageAsync(It.Is(SameMessageAs(
                    expectedLabWorkflowMessage))),
                        Times.Once);

            this.queueBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
