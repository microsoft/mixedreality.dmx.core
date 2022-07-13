// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Foundations.Labs.Exceptions;
using DMX.Core.Api.Models.Orchestrations.Labs.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations
{
    public partial class LabOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowOrchestrationValidationExceptionOnAddIfNullAndLogItAsync()
        {
            // given
            Lab nullLab = null;
            Lab inputLab = nullLab;
            var nullLabException = new NullLabException();

            var expectedLabOrchestrationValidationException =
                new LabOrchestrationValidationException(nullLabException);

            // when
            ValueTask<Lab> addLabTask = this.labOrchestrationService.AddLabAsync(inputLab);

            LabOrchestrationValidationException actualLabOrchestrationValidationException =
                await Assert.ThrowsAsync<LabOrchestrationValidationException>(
                    addLabTask.AsTask);

            // then
            actualLabOrchestrationValidationException
                .Should().BeEquivalentTo(
                    expectedLabOrchestrationValidationException);

            this.labServiceMock.Verify(service =>
                service.AddLabAsync(It.IsAny<Lab>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabOrchestrationValidationException))),
                        Times.Once);

            this.labServiceMock.VerifyNoOtherCalls();
            this.externalLabServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowOrchestrationValidationExceptionOnAddIfLabIsInvalidAndLogItAsync(
            string invalidString)
        {
            // given
            var invalidLab = new Lab
            {
                Name = invalidString,
                Description = invalidString,
                ExternalId = invalidString,
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

            var expectedLabOrchestrationValidationException =
                new LabOrchestrationValidationException(invalidLabException);

            // when
            ValueTask<Lab> actualLabTask =
                this.labOrchestrationService.AddLabAsync(invalidLab);

            LabOrchestrationValidationException actualLabOrchestrationValidationException =
                await Assert.ThrowsAsync<LabOrchestrationValidationException>(
                    actualLabTask.AsTask);

            // then
            actualLabOrchestrationValidationException.Should().BeEquivalentTo(
                expectedLabOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabOrchestrationValidationException))),
                        Times.Once);

            this.labServiceMock.Verify(service =>
                service.AddLabAsync(It.IsAny<Lab>()),
                    Times.Never);

            this.labServiceMock.VerifyNoOtherCalls();
            this.externalLabServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOrchestrationValidationExceptionOnAddIfLabStatusIsInvalidAndLogItAsync()
        {
            // given
            Lab randomLab = CreateRandomLab();
            Lab invalidLab = randomLab;
            invalidLab.Status = GetInvalidEnum<LabStatus>();

            var invalidLabException =
                new InvalidLabException();

            invalidLabException.AddData(
                key: nameof(Lab.Status),
                values: "Value is not recognized");

            var expectedLabOrchestrationValidationException =
                new LabOrchestrationValidationException(invalidLabException);

            // when
            ValueTask<Lab> addLabTask =
                this.labOrchestrationService.AddLabAsync(invalidLab);

            LabOrchestrationValidationException actualLabOrchestrationValidationException = 
                await Assert.ThrowsAsync<LabOrchestrationValidationException>(
                    addLabTask.AsTask);

            // then
            actualLabOrchestrationValidationException
                .Should().BeEquivalentTo(
                    expectedLabOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabOrchestrationValidationException))),
                        Times.Once);

            this.labServiceMock.Verify(service =>
                service.AddLabAsync(It.IsAny<Lab>()),
                    Times.Never);

            this.labServiceMock.VerifyNoOtherCalls();
            this.externalLabServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
