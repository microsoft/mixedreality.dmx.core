// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

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
        public async void ShouldThrowValidationExceptionOnAddIfNullAndLogItAsync()
        {
            // given
            Lab nullLab = null;
            var nullLabException = new NullLabException();

            var expectedLabValidationException =
                new LabValidationException(nullLabException);

            // when
            ValueTask<Lab> addLabTask = this.labService.AddLabAsync(nullLab);

            LabValidationException actualLabValidationException =
                await Assert.ThrowsAsync<LabValidationException>(
                    addLabTask.AsTask);

            // then
            actualLabValidationException.Should().BeEquivalentTo(
                expectedLabValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabAsync(It.IsAny<Lab>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfLabIsInvalidAndLogItAsync(
            string invalidString)
        {
            // given
            var invalidLab = new Lab
            {
                Name = invalidString,
                Description = invalidString,
                ExternalId = invalidString
            };

            var invalidLabException = new InvalidLabException();

            invalidLabException.AddData(
                key: nameof(Lab.Id),
                values: "Id is required");

            invalidLabException.AddData(
                key: nameof(Lab.ExternalId),
                values: "Id is required");

            invalidLabException.AddData(
                key: nameof(Lab.Name),
                values: "Text is required");

            invalidLabException.AddData(
                key: nameof(Lab.Description),
                values: "Text is required");

            var expectedLabValidationException =
                new LabValidationException(invalidLabException);

            // when
            ValueTask<Lab> addLabTask =
                this.labService.AddLabAsync(invalidLab);

            LabValidationException actualLabValidationException =
                await Assert.ThrowsAsync<LabValidationException>(
                    addLabTask.AsTask);

            // then
            actualLabValidationException.Should().BeEquivalentTo(
                expectedLabValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabAsync(It.IsAny<Lab>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfLabStatusIsInvalidAndLogItAsync()
        {
            // given
            Lab randomLab = CreateRandomLab();
            Lab invalidLab = randomLab;
            invalidLab.Status = GetInvalidEnum<LabStatus>();

            var invalidLabException =
                new InvalidLabException();

            invalidLabException.AddData(
                nameof(Lab.Status),
                "Value is not recognized");

            var expectedLabValidationException =
                new LabValidationException(invalidLabException);

            // when
            ValueTask<Lab> addLabTask =
                this.labService.AddLabAsync(invalidLab);

            LabValidationException actualLabValidationException =
                await Assert.ThrowsAsync<LabValidationException>(
                    addLabTask.AsTask);

            // then
            actualLabValidationException.Should().BeEquivalentTo(
                expectedLabValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabAsync(It.IsAny<Lab>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
