// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabCommands
{
    public partial class LabCommandServiceTests
    {
        [Fact]
        public async Task ShouldThrowLabCommandValidationExceptionOnModifyIfInputLabCommandIsNullAndLogItAsync()
        {
            // given
            LabCommand nullLabCommand = null;

            var nullLabCommandException =
                new NullLabCommandException();

            var exptectedLabCommandValidationException =
                new LabCommandValidationException(nullLabCommandException);

            // when
            ValueTask<LabCommand> labCommandTask =
                this.labCommandService.ModifyLabCommandAsync(nullLabCommand);

            LabCommandValidationException actualLabCommandValidationException =
                await Assert.ThrowsAsync<LabCommandValidationException>(
                    labCommandTask.AsTask);

            // then
            actualLabCommandValidationException.Should().BeEquivalentTo(
                exptectedLabCommandValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    exptectedLabCommandValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowLabCommandValidationExceptionOnModifyIfLabCommandIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidLabCommand = new LabCommand
            {
                Arguments = invalidText,
                Notes = invalidText,
                Results = invalidText,
            };

            var invalidLabCommandException = new InvalidLabCommandException();

            invalidLabCommandException.AddData(
                key: nameof(LabCommand.Arguments),
                values: "Text is required");

            invalidLabCommandException.AddData(
                key: nameof(LabCommand.Id),
                values: "Id is required");

            invalidLabCommandException.AddData(
                key: nameof(LabCommand.LabId),
                values: "Id is required");

            invalidLabCommandException.AddData(
                key: nameof(LabCommand.CreatedDate),
                values: "Date is required");

            invalidLabCommandException.AddData(
                key: nameof(LabCommand.UpdatedDate),
                values: "Date is required");

            var expectedLabCommandValidationException =
                    new LabCommandValidationException(invalidLabCommandException);

            // when
            ValueTask<LabCommand> modifyLabCommandTask =
                this.labCommandService.ModifyLabCommandAsync(invalidLabCommand);

            LabCommandValidationException actualLabCommandValidationException =
                await Assert.ThrowsAsync<LabCommandValidationException>(
                    modifyLabCommandTask.AsTask);

            // then
            actualLabCommandValidationException.Should().BeEquivalentTo(
                expectedLabCommandValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfLabCommandStatusOrTypeIsInvalidAndLogItAsync()
        {
            // given
            LabCommand randomLabCommand = CreateRandomLabCommand();
            LabCommand invalidLabCommand = randomLabCommand;
            invalidLabCommand.Status = GetInvalidEnum<CommandStatus>();
            invalidLabCommand.Type = GetInvalidEnum<CommandType>();

            DateTimeOffset randomDate =
                GetValidDateTimeOffset(
                    invalidLabCommand.CreatedDate,
                    new TimeSpan(0, 1, 0));

            DateTimeOffset currentDate = randomDate;
            invalidLabCommand.UpdatedDate = currentDate;
            LabCommand updatedLabCommand = invalidLabCommand.DeepClone();
            LabCommand expectedLabCommand = updatedLabCommand.DeepClone();

            var invalidLabCommandException = new InvalidLabCommandException();

            invalidLabCommandException.AddData(
                key: nameof(LabCommand.Status),
                values: "Value is not recognized");

            invalidLabCommandException.AddData(
                key: nameof(LabCommand.Type),
                values: "Value is not recognized");

            var exptectedLabCommandValidationException =
                new LabCommandValidationException(invalidLabCommandException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(currentDate);

            // when
            ValueTask<LabCommand> labCommandTask =
                this.labCommandService.ModifyLabCommandAsync(invalidLabCommand);

            LabCommandValidationException actualLabCommandValidationException =
                await Assert.ThrowsAsync<LabCommandValidationException>(
                    labCommandTask.AsTask);

            // then
            actualLabCommandValidationException.Should().BeEquivalentTo(
                exptectedLabCommandValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    exptectedLabCommandValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            LabCommand randomLabCommand = CreateRandomLabCommand(randomDateTime);
            LabCommand invalidLabCommand = randomLabCommand;
            var invalidLabCommandException = new InvalidLabCommandException();

            invalidLabCommandException.AddData(
                key: nameof(LabCommand.UpdatedDate),
                values: $"Date is the same as {nameof(LabCommand.CreatedDate)}");

            var expectedLabCommandException =
                new LabCommandValidationException(invalidLabCommandException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<LabCommand> modifyLabCommandTask =
                this.labCommandService.ModifyLabCommandAsync(invalidLabCommand);

            LabCommandValidationException actualLabCommandValidationException =
                await Assert.ThrowsAsync<LabCommandValidationException>(
                    modifyLabCommandTask.AsTask);

            // then
            actualLabCommandValidationException.Should().BeEquivalentTo(expectedLabCommandException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsBeforeCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            LabCommand randomLabCommand = CreateRandomLabCommand(randomDateTime);
            LabCommand inputLabCommand = randomLabCommand;
            int randomNumber = GetRandomNumber();
            inputLabCommand.CreatedDate = randomDateTime.AddMinutes(randomNumber);

            var invalidLabCommandException =
                new InvalidLabCommandException();

            invalidLabCommandException.AddData(
                key: nameof(LabCommand.UpdatedDate),
                values: $"Date can not be before {nameof(LabCommand.CreatedDate)}");

            var expectedLabCommandValidationException =
                new LabCommandValidationException(invalidLabCommandException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<LabCommand> labCommandTask =
                this.labCommandService.ModifyLabCommandAsync(inputLabCommand);

            LabCommandValidationException actualLabCommandValidationException =
                await Assert.ThrowsAsync<LabCommandValidationException>(
                    labCommandTask.AsTask);

            // then
            actualLabCommandValidationException.Should().BeEquivalentTo(
                expectedLabCommandValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            LabCommand randomLabCommand = CreateRandomLabCommand();
            LabCommand storageLabCommand = randomLabCommand;
            LabCommand invalidLabCommand = randomLabCommand.DeepClone();
            invalidLabCommand.CreatedDate = GetRandomDateTimeOffset();

            DateTimeOffset randomDate =
                GetValidDateTimeOffset(
                    invalidLabCommand.CreatedDate,
                    new TimeSpan(0, 1, 0));

            DateTimeOffset currentDate = randomDate;
            invalidLabCommand.UpdatedDate = currentDate;
            Guid labCommandId = invalidLabCommand.Id;
            var invalidLabCommandException = new InvalidLabCommandException();

            invalidLabCommandException.AddData(
                key: nameof(LabCommand.CreatedDate),
                values: $"Date is not the same as stored {nameof(LabCommand.CreatedDate)}");

            var expectedLabCommandValidationException =
                new LabCommandValidationException(invalidLabCommandException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(currentDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabCommandByIdAsync(labCommandId))
                    .ReturnsAsync(storageLabCommand);

            // when
            ValueTask<LabCommand> modifyLabCommandTask =
                this.labCommandService.ModifyLabCommandAsync(invalidLabCommand);

            LabCommandValidationException actualLabCommandValidationException =
                await Assert.ThrowsAsync<LabCommandValidationException>(
                    modifyLabCommandTask.AsTask);

            // then
            actualLabCommandValidationException.Should().BeEquivalentTo(
                expectedLabCommandValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(labCommandId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.UpdateLabCommandAsync(It.IsAny<LabCommand>()),
                   Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync()
        {
            // given
            LabCommand randomLabCommand = CreateRandomLabCommand();
            LabCommand invalidLabCommand = randomLabCommand;

            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset(
                earliestDate: invalidLabCommand.CreatedDate);

            invalidLabCommand.UpdatedDate = randomDateTimeOffset;

            var invalidLabCommandException =
                new InvalidLabCommandException();

            invalidLabCommandException.AddData(
                key: nameof(LabCommand.UpdatedDate),
                values: "Date is not recent");

            var expectedLabCommandValidationException =
                new LabCommandValidationException(invalidLabCommandException);

            DateTimeOffset invalidDateTimeOffset =
                GetInvalidDateTimeOffset(invalidLabCommand.UpdatedDate, new TimeSpan(0, 1, 0));

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(invalidDateTimeOffset);

            // when
            ValueTask<LabCommand> labCommandTask =
                this.labCommandService.ModifyLabCommandAsync(invalidLabCommand);

            LabCommandValidationException actualLabCommandValidationException =
                await Assert.ThrowsAsync<LabCommandValidationException>(
                    labCommandTask.AsTask);

            // then
            actualLabCommandValidationException.Should().BeEquivalentTo(
                expectedLabCommandValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfLabCommandIsNotFoundAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            LabCommand randomLabCommand = CreateRandomLabCommand(randomDateTimeOffset);
            int randomNumber = GetRandomNumber();
            LabCommand invalidLabCommand = randomLabCommand;
            invalidLabCommand.UpdatedDate = invalidLabCommand.CreatedDate.AddSeconds(randomNumber);
            Guid notFoundId = invalidLabCommand.Id;
            LabCommand nullLabCommand = null;

            var notFoundLabCommandException =
                new NotFoundLabCommandException(notFoundId);

            var expectedLabCommandValidationException =
                new LabCommandValidationException(notFoundLabCommandException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabCommandByIdAsync(notFoundId))
                    .ReturnsAsync(nullLabCommand);

            // when
            ValueTask<LabCommand> modifyLabCommandTask =
                this.labCommandService.ModifyLabCommandAsync(invalidLabCommand);

            LabCommandValidationException actualLabCommandValidationException =
                await Assert.ThrowsAsync<LabCommandValidationException>(
                    modifyLabCommandTask.AsTask);

            // then
            actualLabCommandValidationException.Should().BeEquivalentTo(
                expectedLabCommandValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}