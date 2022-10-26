// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations.LabWorkflows
{
    public partial class LabWorkflowOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldSubmitLabWorkflowAsync()
        {
            // given
            LabWorkflow randomLabWorkflow = CreateRandomLabWorkflow();
            LabWorkflow inputLabWorkflow = randomLabWorkflow;
            LabWorkflow returnedLabWorkflow = inputLabWorkflow;
            List<LabWorkflowCommand> inputLabWorkflowCommands = inputLabWorkflow.Commands;
            LabWorkflow eventLabWorkflow = inputLabWorkflow;
            LabWorkflow expectedLabWorkflow = inputLabWorkflow.DeepClone();
            var mockSequence = new MockSequence();

            this.labWorkflowServiceMock.InSequence(mockSequence).Setup(service =>
                service.AddLabWorkflowAsync(inputLabWorkflow))
                    .ReturnsAsync(returnedLabWorkflow);

            inputLabWorkflowCommands.ForEach(inputLabWorkflowCommand =>
                this.labWorkflowCommandServiceMock.InSequence(mockSequence).Setup(service =>
                    service.AddLabWorkflowCommandAsync(inputLabWorkflowCommand))
                        .ReturnsAsync(inputLabWorkflowCommand));

            this.labWorkflowEventServiceMock.InSequence(mockSequence).Setup(service =>
                service.AddLabWorkflowEventAsync(inputLabWorkflow))
                    .ReturnsAsync(eventLabWorkflow);

            // when
            LabWorkflow actualLabWorkflow =
                await this.labWorkflowOrchestrationService.SubmitLabWorkflowAsync(inputLabWorkflow);

            // then
            actualLabWorkflow.Should().BeEquivalentTo(expectedLabWorkflow);

            this.labWorkflowServiceMock.Verify(service =>
                service.AddLabWorkflowAsync(inputLabWorkflow),
                    Times.Once);

            inputLabWorkflowCommands.ForEach(inputLabWorkflowCommand =>
                this.labWorkflowCommandServiceMock.Verify(service =>
                    service.AddLabWorkflowCommandAsync(inputLabWorkflowCommand),
                        Times.Once));

            this.labWorkflowEventServiceMock.Verify(service =>
                service.AddLabWorkflowEventAsync(inputLabWorkflow),
                    Times.Once);

            this.labWorkflowServiceMock.VerifyNoOtherCalls();
            this.labWorkflowCommandServiceMock.VerifyNoOtherCalls();
            this.labWorkflowEventServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
