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
            DateTimeOffset currentDateTimeOffset = randomDateTimeOffset;
            int randomNegativeNumber = GetRandomNegativeNumber();

            DateTimeOffset labWorkflowCommandCreatedDateTimeOffset =
                currentDateTimeOffset.AddMinutes(randomNegativeNumber);

            LabWorkflowCommand storageLabWorkflowCommand =
                CreateRandomLabWorkflowCommand(labWorkflowCommandCreatedDateTimeOffset);

            LabWorkflowCommand inputLabWorkflowCommand =
                CreateRandomLabWorkflowCommand(labWorkflowCommandCreatedDateTimeOffset);

            inputLabWorkflowCommand.UpdatedDate = currentDateTimeOffset;
            LabWorkflowCommand updatedLabWorkflowCommand = inputLabWorkflowCommand;
            LabWorkflowCommand expectedLabWorkflowCommand = updatedLabWorkflowCommand.DeepClone();
            Guid inputLabCommandWorkflowId = inputLabWorkflowCommand.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(inputLabWorkflowCommand.Id))
                    .ReturnsAsync(storageLabWorkflowCommand);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateLabWorkflowCommandAsync(inputLabWorkflowCommand))
                    .ReturnsAsync(updatedLabWorkflowCommand);

            // when
            LabWorkflowCommand actualLabWorkflowCommand =
                await this.labWorkflowCommandService.ModifyLabWorkflowCommand(inputLabWorkflowCommand);

            // then
            actualLabWorkflowCommand.Should().BeEquivalentTo(expectedLabWorkflowCommand);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(inputLabCommandWorkflowId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabWorkflowCommandAsync(inputLabWorkflowCommand),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
