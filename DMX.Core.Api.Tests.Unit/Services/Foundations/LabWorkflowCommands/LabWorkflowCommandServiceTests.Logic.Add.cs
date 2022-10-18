// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandServiceTests
    {
        [Fact]
        public async Task ShouldAddLabWorkflowCommandAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            LabWorkflowCommand randomLabWorkflow = CreateRandomLabWorkflowCommand();
            LabWorkflowCommand inputLabWorkflowCommand = randomLabWorkflow;
            LabWorkflowCommand insertedLabWorkflowCommand = inputLabWorkflowCommand;
            LabWorkflowCommand expectedLabWorkflowCommand = inputLabWorkflowCommand.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLabWorkflowCommandAsync(inputLabWorkflowCommand))
                    .ReturnsAsync(insertedLabWorkflowCommand);

            // when
            LabWorkflowCommand actualLabWorkflow =
                await this.labWorkflowService.AddLabWorkflowCommandAsync(inputLabWorkflowCommand);

            // then
            actualLabWorkflow.Should().BeEquivalentTo(expectedLabWorkflowCommand);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabWorkflowCommandAsync(inputLabWorkflowCommand),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
