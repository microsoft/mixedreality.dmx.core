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
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidId = Guid.Empty;
            var invalidLabException = new InvalidLabException();

            invalidLabException.AddData(
                key: nameof(Lab.Id),
                values: "Id is required");

            var expectedLabValidationException =
                new LabValidationException(invalidLabException);

            // when
            ValueTask<Lab> removeLabTask = this.labService.RemoveLabByIdAsync(invalidId);

            LabValidationException actualLabValidationException =
                await Assert.ThrowsAsync<LabValidationException>(
                    removeLabTask.AsTask);

            // then
            actualLabValidationException.Should().BeEquivalentTo(
                expectedLabValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabByIdWithoutDevicesAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLabAsync(It.IsAny<Lab>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfLabIsNullAndLogItAsync()
        {
            // given
            Lab noLab = null;
            var someId = Guid.NewGuid();
            var notFoundLabException = new NotFoundLabException(someId);

            var expectedLabValidationException =
                new LabValidationException(notFoundLabException);

            storageBrokerMock.Setup(broker =>
                broker.SelectLabByIdWithoutDevicesAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noLab);

            // when
            ValueTask<Lab> removeLabTask = this.labService.RemoveLabByIdAsync(someId);

            LabValidationException actualLabValidationException =
                await Assert.ThrowsAsync<LabValidationException>(
                    removeLabTask.AsTask);

            // then
            actualLabValidationException.Should().BeEquivalentTo(
                expectedLabValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabByIdWithoutDevicesAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLabAsync(It.IsAny<Lab>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
