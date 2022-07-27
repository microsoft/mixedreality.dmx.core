// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Foundations.Labs.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.Labs
{
    public partial class LabServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionIfLabIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidLabId = Guid.Empty;
            var invalidLabException = new InvalidLabException();

            invalidLabException.AddData(
                key: nameof(Lab.Id),
                values: "Id is required");

            var expectedLabValidationException =
                new LabValidationException(invalidLabException);

            // when
            ValueTask<Lab> retrieveLabByIdTask =
                this.labService.RetrieveLabByIdAsync(invalidLabId);

            LabValidationException actualLabValidationException =
                await Assert.ThrowsAsync<LabValidationException>(
                    retrieveLabByIdTask.AsTask);

            // then
            actualLabValidationException.Should().BeEquivalentTo(
                expectedLabValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfLabIsNotFoundAndLogitAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid nonExistentLabId = randomId;
            Lab noLab = null;

            var notFoundLabException =
                new NotFoundLabException(nonExistentLabId);

            var expectedLabValidationException =
                new LabValidationException(notFoundLabException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabByIdAsync(nonExistentLabId))
                    .ReturnsAsync(noLab);

            // when
            ValueTask<Lab> retrieveLabByIdTask =
                this.labService.RetrieveLabByIdAsync(nonExistentLabId);

            LabValidationException actualLabValidationException =
                await Assert.ThrowsAsync<LabValidationException>(
                    retrieveLabByIdTask.AsTask);

            // then
            actualLabValidationException.Should().BeEquivalentTo(
                expectedLabValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
