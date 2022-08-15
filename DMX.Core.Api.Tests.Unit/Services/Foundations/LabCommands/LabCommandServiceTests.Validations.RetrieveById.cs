// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabCommands
{
    public partial class LabCommandServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionIfLabCommandIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidLabCommandId = Guid.Empty;
            var invalidLabCommandException = new InvalidLabCommandException();

            invalidLabCommandException.AddData(
                key: nameof(LabCommand.Id),
                values: "Id is required");

            var expectedLabCommandValidationException =
                new LabCommandValidationException(invalidLabCommandException);

            // when
            ValueTask<LabCommand> retrieveLabCommandByIdTask =
                this.labCommandService.RetrieveLabCommandByIdAsync(invalidLabCommandId);

            LabCommandValidationException actualLabCommandValidationException =
                await Assert.ThrowsAsync<LabCommandValidationException>(
                    retrieveLabCommandByIdTask.AsTask);

            // then
            actualLabCommandValidationException.Should().BeEquivalentTo(
                expectedLabCommandValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfLabCommandIsNotFoundAndLogItAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid notFoundLabCommandId = randomId;
            LabCommand notFoundLab = null;

            var notFoundLabCommandException =
                new NotFoundLabCommandException(notFoundLabCommandId);

            var expectedLabCommandValidationException =
                new LabCommandValidationException(notFoundLabCommandException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabCommandByIdAsync(notFoundLabCommandId))
                    .ReturnsAsync(notFoundLab);

            // when
            ValueTask<LabCommand> retrieveLabCommandByIdTask =
                this.labCommandService.RetrieveLabCommandByIdAsync(
                    notFoundLabCommandId);

            LabCommandValidationException actualLabCommandValidationException =
                await Assert.ThrowsAsync<LabCommandValidationException>(
                    retrieveLabCommandByIdTask.AsTask);

            // then
            actualLabCommandValidationException.Should().BeEquivalentTo(
                expectedLabCommandValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
