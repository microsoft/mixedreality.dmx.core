// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfNullAndLogItAsync()
        {
            // given
            LabWorkflowCommand nullLabWorkflowCommand = null;
            var nullLabWorkflowCommandException = new NullLabWorkflowCommandException();

            var expectedLabWorkflowCommandValidationException =
                new LabWorkflowCommandValidationException(nullLabWorkflowCommandException);

            // when
            ValueTask<LabWorkflowCommand> addLabWorkflowCommandTask =
                this.labWorkflowCommandService.AddLabWorkflowCommandAsync(nullLabWorkflowCommand);

            LabWorkflowCommandValidationException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandValidationException>(
                    addLabWorkflowCommandTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabWorkflowCommandAsync(
                    It.IsAny<LabWorkflowCommand>()),
                        Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfLabWorkflowCommandIsInvalidAndLogItAsync(
            string invalidString)
        {
            // given
            var invalidLabWorkflowCommand = new LabWorkflowCommand
            {
                Notes = invalidString,
                Arguments = invalidString,
            };

            var invalidLabWorkflowCommandException = new InvalidLabWorkflowCommandException();

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.Id),
                values: "Id is required");

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.WorkflowId),
                values: "Id is required");

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.Arguments),
                values: "Text is required");

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.CreatedBy),
                values: "User is required");

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.UpdatedBy),
                values: "User is required");

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.CreatedDate),
                values: "Date is required");

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.UpdatedDate),
                values: "Date is required");

            var expectedLabWorkflowCommandValidationException =
                new LabWorkflowCommandValidationException(invalidLabWorkflowCommandException);

            // when
            ValueTask<LabWorkflowCommand> addLabWorkflowTask =
                this.labWorkflowCommandService.AddLabWorkflowCommandAsync(invalidLabWorkflowCommand);

            LabWorkflowCommandValidationException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandValidationException>(
                    addLabWorkflowTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfLabWorkflowCommandTypeOrCommandStatusIsInvalidAndLogItAsync()
        {
            // given
            var dateTime = GetRandomDateTimeOffset();
            var randomLabWorkflowCommand = CreateRandomLabWorkflowCommand(dateTime);
            LabWorkflowCommand invalidLabWorkflowCommand = randomLabWorkflowCommand;
            randomLabWorkflowCommand.Type = GetInvalidEnum<CommandType>();
            randomLabWorkflowCommand.Status = GetInvalidEnum<CommandStatus>();

            var invalidLabWorkflowCommandException = new InvalidLabWorkflowCommandException();

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.Type),
                values: "Value is not recognized");

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.Status),
                values: "Value is not recognized");

            var expectedLabWorkflowCommandValidationException =
                new LabWorkflowCommandValidationException(invalidLabWorkflowCommandException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<LabWorkflowCommand> addLabWorkflowTask =
                this.labWorkflowCommandService.AddLabWorkflowCommandAsync(invalidLabWorkflowCommand);

            LabWorkflowCommandValidationException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandValidationException>(
                    addLabWorkflowTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            int randomNumber = GetRandomNumber();
            LabWorkflowCommand randomLabWorkflowCommand = CreateRandomLabWorkflowCommand(randomDateTime);
            LabWorkflowCommand invalidLabWorkflowCommand = randomLabWorkflowCommand;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            invalidLabWorkflowCommand.UpdatedDate =
                invalidLabWorkflowCommand.UpdatedDate.AddDays(randomNumber);

            var invalidLabWorkflowCommandException = new InvalidLabWorkflowCommandException();

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.UpdatedDate),
                values: $"Date is not the same as {nameof(LabWorkflowCommand.CreatedDate)}");

            var expectedLabWorkflowCommandValidationException =
                new LabWorkflowCommandValidationException(invalidLabWorkflowCommandException);

            // when
            ValueTask<LabWorkflowCommand> addLabWorkflowTask =
                this.labWorkflowCommandService.AddLabWorkflowCommandAsync(invalidLabWorkflowCommand);

            LabWorkflowCommandValidationException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandValidationException>(
                    addLabWorkflowTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int invalidSeconds)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            LabWorkflowCommand randomLabWorkflowCommand = CreateRandomLabWorkflowCommand();
            LabWorkflowCommand invalidLabWorkflowCommand = randomLabWorkflowCommand;

            invalidLabWorkflowCommand.CreatedDate =
                randomDateTimeOffset.AddSeconds(invalidSeconds);

            invalidLabWorkflowCommand.UpdatedDate =
                invalidLabWorkflowCommand.CreatedDate;

            var invalidLabWorkflowCommandException = new InvalidLabWorkflowCommandException();

            invalidLabWorkflowCommandException.AddData(
                key: nameof(LabWorkflowCommand.CreatedDate),
                values: "Date is not recent");

            var expectedLabWorkflowCommandValidationException =
                new LabWorkflowCommandValidationException(invalidLabWorkflowCommandException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<LabWorkflowCommand> addLabWorkflowCommandTask =
                this.labWorkflowCommandService.AddLabWorkflowCommandAsync(invalidLabWorkflowCommand);

            LabWorkflowCommandValidationException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandValidationException>(
                    addLabWorkflowCommandTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabWorkflowCommandAsync(
                    It.IsAny<LabWorkflowCommand>()),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
