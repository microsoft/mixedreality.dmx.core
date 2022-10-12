// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldModifyLabWorkflowCommandAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            LabWorkflowCommand inputLabWorkflowCommand = CreateRandomLabWorkflowCommand();
            Guid inputLabCommandWorkflowId = inputLabWorkflowCommand.Id;
            LabWorkflowCommand storageLabWorkflowCommand = CreateRandomLabWorkflowCommand();
            storageLabWorkflowCommand.Id = inputLabWorkflowCommand.Id;
            storageLabWorkflowCommand.CreatedDate = inputLabWorkflowCommand.CreatedDate;
            LabWorkflowCommand updatedLabWorkflowCommand = inputLabWorkflowCommand.DeepClone();
            LabWorkflowCommand expectedLabWorkflowCommand = updatedLabWorkflowCommand.DeepClone();

            this.datetimeBroker.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBroker.Setup(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(inputLabWorkflowCommand.Id))
                    .ReturnsAsync(storageLabWorkflowCommand);

            this.storageBroker.Setup(broker =>
                broker.UpdateLabWorkflowCommandAsync(inputLabWorkflowCommand))
                    .ReturnsAsync(updatedLabWorkflowCommand);

            // when
            LabWorkflowCommand actualLabWorkflowCommand =
                await this.labWorkflowCommandService.ModifyLabWorkflowCommand(inputLabWorkflowCommand);

            // then
            actualLabWorkflowCommand.Should().BeEquivalentTo(expectedLabWorkflowCommand);

            this.datetimeBroker.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBroker.Verify(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(inputLabCommandWorkflowId),
                    Times.Never);

            this.storageBroker.Verify(broker =>
                broker.UpdateLabWorkflowCommandAsync(inputLabWorkflowCommand),
                    Times.Once);

            this.datetimeBroker.VerifyNoOtherCalls();
            this.storageBroker.VerifyNoOtherCalls();
            this.loggingBroker.VerifyNoOtherCalls();
        }
    }
}
