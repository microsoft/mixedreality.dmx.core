// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflows
{
    public partial class LabWorkflowTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfNullAndLogItAsync()
        {
            // given
            LabWorkflow nullLabWorkflow = null;
            var nullLabWorkflowException = new NullLabWorkflowException();

            var expectedLabWorkflowValidationException =
                new LabWorkflowValidationException(nullLabWorkflowException);

            // when
            ValueTask<LabWorkflow> addLabWorkflowTask =
                this.labWorkflowService.AddLabWorkflowAsync(nullLabWorkflow);

            LabWorkflowValidationException actualLabWorkflowValidationException =
                await Assert.ThrowsAsync<LabWorkflowValidationException>(
                    addLabWorkflowTask.AsTask);

            // then
            actualLabWorkflowValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabWorkflowAsync(
                    It.IsAny<LabWorkflow>()),
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
            var invalidLabWorkflow = new LabWorkflow
            {
                Name = invalidString,
                Owner = invalidString,
                Results = invalidString,
            };

            var invalidLabWorkflowException = new InvalidLabWorkflowException();

            invalidLabWorkflowException.AddData(
                key: nameof(LabWorkflow.Id),
                values: "Id is required");

            invalidLabWorkflowException.AddData(
                key: nameof(LabWorkflow.Name),
                values: "Text is required");

            invalidLabWorkflowException.AddData(
                key: nameof(LabWorkflow.Owner),
                values: "Text is required");

            invalidLabWorkflowException.AddData(
                key: nameof(LabWorkflow.CreatedBy),
                values: "User is required");

            invalidLabWorkflowException.AddData(
                key: nameof(LabWorkflow.UpdatedBy),
                values: "User is required");

            invalidLabWorkflowException.AddData(
                key: nameof(LabWorkflow.CreatedDate),
                values: "Date is required");

            invalidLabWorkflowException.AddData(
                key: nameof(LabWorkflow.UpdatedDate),
                values: "Date is required");

            var expectedLabWorkflowValidationException =
                new LabWorkflowValidationException(invalidLabWorkflowException);

            // when
            ValueTask<LabWorkflow> addLabWorkflowTask =
                this.labWorkflowService.AddLabWorkflowAsync(invalidLabWorkflow);

            LabWorkflowValidationException actualLabWorkflowValidationException =
                await Assert.ThrowsAsync<LabWorkflowValidationException>(
                    addLabWorkflowTask.AsTask);

            // then
            actualLabWorkflowValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabWorkflowAsync(It.IsAny<LabWorkflow>()),
                    Times.Never);
        }
    }
}
