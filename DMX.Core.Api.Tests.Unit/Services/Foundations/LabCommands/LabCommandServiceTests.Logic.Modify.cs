// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabCommands
{
    public partial class LabCommandServiceTests
    {
        [Fact]
        public async Task ShouldModifyLabCommandAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset currentDateTimeOffset = randomDateTimeOffset;
            int randomNegativeNumber = GetRandomNegativeNumber();

            DateTimeOffset labCommandCreatedDateTimeOffset =
                currentDateTimeOffset.AddMinutes(randomNegativeNumber);

            LabCommand storageLabCommand = CreateRandomLabCommand(labCommandCreatedDateTimeOffset);
            LabCommand inputLabCommand = CreateRandomLabCommand(labCommandCreatedDateTimeOffset);
            inputLabCommand.UpdatedDate = currentDateTimeOffset;
            LabCommand updatedLabCommand = inputLabCommand;
            LabCommand expectedLabCommand = updatedLabCommand.DeepClone();
            Guid labCommandId = inputLabCommand.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(currentDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabCommandByIdAsync(labCommandId))
                    .ReturnsAsync(storageLabCommand);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateLabCommandAsync(inputLabCommand))
                    .ReturnsAsync(updatedLabCommand);

            // when
            LabCommand actualLabCommand =
                await this.labCommandService.ModifyLabCommandAsync(inputLabCommand);

            // then
            actualLabCommand.Should().BeEquivalentTo(expectedLabCommand);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(labCommandId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabCommandAsync(inputLabCommand),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}