// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;
using FluentAssertions;
using Force.DeepCloner;
using Microsoft.Extensions.Azure;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandServiceTests
    {
        [Fact]
        public async Task ShouldThrowLabWorkflowCommandValidationExceptionOnModifyIfInputIsNullAndLogItAsync()
        {
            // given
            LabWorkflowCommand nullLabWorkflowCommand = null;

            var nullLabWorkflowCommandException =
                new NullLabWorkflowCommandException();

            var expectedLabWorkflowCommandValidationException =
                new LabWorkflowCommandValidationException(nullLabWorkflowCommandException);

            // when
            ValueTask<LabWorkflowCommand> modifyLabCommandTask =
                this.labWorkflowCommandService.ModifyLabWorkflowCommand(nullLabWorkflowCommand);

            LabWorkflowCommandValidationException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandValidationException>(
                    modifyLabCommandTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.datetimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task
            ShouldThrowLabWorkflowCommandValidationExceptionOnModifyIfLabWorkflowCommandIsInvalidAndLogItAsync(string invalidData)
        {
            // given
            LabWorkflowCommand invalidLabWorkflowCommand = new LabWorkflowCommand
            {
                Arguments = invalidData,
                Notes = invalidData,
                Results = invalidData,
            };

            var invalidLabWorkflowCommandException = new InvalidLabWorkflowCommandException();

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.Id),
                values: "Id is required");

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.LabId),
                values: "Id is required");

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.WorkflowId),
                values: "Id is required");

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.Arguments),
                values: "Text is required");

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.CreatedDate),
                values: "Date is required");

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.UpdatedDate),
                values: "Date is required");

            var expectedabWorkflowCommandValidationException =
                new LabWorkflowCommandValidationException(invalidLabWorkflowCommandException);

            // when
            ValueTask<LabWorkflowCommand> modifyLabWorkflowCommandTask =
                this.labWorkflowCommandService.ModifyLabWorkflowCommand(invalidLabWorkflowCommand);

            LabWorkflowCommandValidationException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandValidationException>(
                    modifyLabWorkflowCommandTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(
                expectedabWorkflowCommandValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedabWorkflowCommandValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.datetimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.datetimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfLabWorkflowCommandStatusOrTypeIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            int minutesInPast = GetRandomNegativeNumber();
            LabWorkflowCommand randomLabWorkflowCommand = CreateRandomLabWorkflowCommand(randomDateTimeOffset);
            LabWorkflowCommand invalidLabWorkflowCommand = randomLabWorkflowCommand;
            invalidLabWorkflowCommand.Status = GetInvalidEnum<CommandStatus>();
            invalidLabWorkflowCommand.Type = GetInvalidEnum<CommandType>();

            invalidLabWorkflowCommand.CreatedDate =
                invalidLabWorkflowCommand.UpdatedDate.AddMinutes(minutesInPast);

            var invalidLabWorkflowCommandException =
                new InvalidLabWorkflowCommandException();

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.Status),
                values: "Value is not recognized");

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.Type),
                values: "Value is not recognized");

            var expectedLabWorkflowCommandValidationException =
                new LabWorkflowCommandValidationException(invalidLabWorkflowCommandException);

            this.datetimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<LabWorkflowCommand> labWorkflowCommandTask =
                this.labWorkflowCommandService.ModifyLabWorkflowCommand(invalidLabWorkflowCommand);

            LabWorkflowCommandValidationException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandValidationException>(
                    labWorkflowCommandTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandValidationException))),
                        Times.Once);

            this.datetimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.datetimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            LabWorkflowCommand randomLabWorkflowCommand = CreateRandomLabWorkflowCommand(randomDateTimeOffset);
            LabWorkflowCommand invalidLabWorkflowCommand = randomLabWorkflowCommand;
            var invalidLabWorkflowCommandException = new InvalidLabWorkflowCommandException();

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.UpdatedDate),
                values: $"Date is the same as {nameof(LabWorkflowCommand.CreatedDate)}");

            var expectedLabWorkflowCommandValidationException = new LabWorkflowCommandValidationException(
                invalidLabWorkflowCommandException);

            this.datetimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<LabWorkflowCommand> modifyLabWorkflowCommandTask =
                this.labWorkflowCommandService.ModifyLabWorkflowCommand(invalidLabWorkflowCommand);

            LabWorkflowCommandValidationException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandValidationException>(
                    modifyLabWorkflowCommandTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(expectedLabWorkflowCommandValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandValidationException))),
                        Times.Once);

            this.datetimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.datetimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsBeforeCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            LabWorkflowCommand randomLabWorkflowCommand = CreateRandomLabWorkflowCommand(randomDateTimeOffset);
            LabWorkflowCommand invalidLabWorkflowCommand = randomLabWorkflowCommand;
            int randomNumber = GetRandomNumber();

            invalidLabWorkflowCommand.CreatedDate =
                randomDateTimeOffset.AddMinutes(randomNumber);

            var invalidLabWorkflowCommandException = new InvalidLabWorkflowCommandException();

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.UpdatedDate),
                values: $"Date cannot be before {nameof(LabWorkflowCommand.CreatedDate)}");

            var expectedLabWorkflowCommandValidationException = new LabWorkflowCommandValidationException(invalidLabWorkflowCommandException);

            this.datetimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<LabWorkflowCommand> modifyLabWorkflowCommandTask =
                this.labWorkflowCommandService.ModifyLabWorkflowCommand(invalidLabWorkflowCommand);

            LabWorkflowCommandValidationException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandValidationException>(
                    modifyLabWorkflowCommandTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(expectedLabWorkflowCommandValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandValidationException))),
                        Times.Once);

            this.datetimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.datetimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedDateIsNotSameAsStorageCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            DateTimeOffset currentDateTime = randomDateTime;
            int randomNegativeNumber = GetRandomNegativeNumber();
            DateTimeOffset labWorkflowCommandCreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);

            LabWorkflowCommand storageLabWorkflowCommand = CreateRandomLabWorkflowCommand(labWorkflowCommandCreatedDate);
            LabWorkflowCommand invalidLabWorkflowCommand = CreateRandomLabWorkflowCommand(currentDateTime);
            invalidLabWorkflowCommand.CreatedDate = currentDateTime.AddSeconds(randomNegativeNumber);

            var invalidLabWorkflowCommandException =
                new InvalidLabWorkflowCommandException();

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.CreatedDate),
                values: $"Date is not the same as stored {nameof(LabWorkflowCommand.CreatedDate)}");

            var expectedLabWorkflowCommandValidationException =
                new LabWorkflowCommandValidationException(invalidLabWorkflowCommandException);

            this.datetimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(invalidLabWorkflowCommand.Id))
                    .ReturnsAsync(storageLabWorkflowCommand);

            // when
            ValueTask<LabWorkflowCommand> modifyLabWorkflowCommandTask =
                this.labWorkflowCommandService.ModifyLabWorkflowCommand(invalidLabWorkflowCommand);

            LabWorkflowCommandValidationException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandValidationException>(modifyLabWorkflowCommandTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.datetimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.datetimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(
            int invalidSeconds)
        {
            // given
            int randomMinutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset currentDateTime = randomDateTimeOffset;
            LabWorkflowCommand randomLabWorkflowCommand = CreateRandomLabWorkflowCommand();
            LabWorkflowCommand invalidLabWorkflowCommand = randomLabWorkflowCommand;

            invalidLabWorkflowCommand.UpdatedDate =
                currentDateTime.AddSeconds(invalidSeconds);

            invalidLabWorkflowCommand.CreatedDate =
                invalidLabWorkflowCommand.UpdatedDate.AddMinutes(randomMinutesInPast);

            var invalidLabWorkflowCommandException =
                new InvalidLabWorkflowCommandException();

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.UpdatedDate),
                values: "Date is not recent");

            var expectedLabWorkflowCommandValidationException = 
                new LabWorkflowCommandValidationException(invalidLabWorkflowCommandException);

            this.datetimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(currentDateTime);

            // when
            ValueTask<LabWorkflowCommand> labWorkflowCommandTask =
                this.labWorkflowCommandService.ModifyLabWorkflowCommand(
                    invalidLabWorkflowCommand);

            LabWorkflowCommandValidationException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandValidationException>(
                    labWorkflowCommandTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandValidationException);

            this.datetimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(
                    It.IsAny<Guid>()),
                        Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabWorkflowCommandAsync(
                    It.IsAny<LabWorkflowCommand>()),
                        Times.Never);

            this.datetimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionIfLabWorkflowCommandNotFoundAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            int randomNegativeNumber = GetRandomNegativeNumber();
            LabWorkflowCommand invalidLabWorkflowCommand = CreateRandomLabWorkflowCommand(randomDateTimeOffset);
            invalidLabWorkflowCommand.CreatedDate = randomDateTimeOffset.AddMinutes(randomNegativeNumber);
            Guid notFoundId = invalidLabWorkflowCommand.Id;
            LabWorkflowCommand nullLabWorkflowCommand = null;

            var notFoundLabWorkflowCommandException =
                new NotFoundLabWorkflowCommandException(notFoundId);

            var expectedLabWorkflowCommandValidationException =
                new LabWorkflowCommandValidationException(notFoundLabWorkflowCommandException);

            this.datetimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(notFoundId))
                    .ReturnsAsync(nullLabWorkflowCommand);

            // when
            ValueTask<LabWorkflowCommand> modifyLabWorkflowCommandTask =
                this.labWorkflowCommandService.ModifyLabWorkflowCommand(
                    invalidLabWorkflowCommand);

            LabWorkflowCommandValidationException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandValidationException>(
                    modifyLabWorkflowCommandTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandValidationException))),
                        Times.Once);

            this.datetimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.datetimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
