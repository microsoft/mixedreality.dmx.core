// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldThrowValidationExceptionIfLabWorkflowIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidLabWorkflowId = Guid.Empty;
            var invalidLabWorkflowException = new InvalidLabWorkflowException();

            invalidLabWorkflowException.AddData(
                key: nameof(LabWorkflow.Id),
                values: "Id is required");

            var expectedLabWorkflowValidationException =
                new LabWorkflowValidationException(invalidLabWorkflowException);

            // when
            ValueTask<LabWorkflow> retrieveLabWorkflowByIdTask =
                this.labWorkflowService.RetrieveLabWorkflowByIdAsync(invalidLabWorkflowId);

            LabWorkflowValidationException actualLabWorkflowValidationException =
                await Assert.ThrowsAsync<LabWorkflowValidationException>(
                    retrieveLabWorkflowByIdTask.AsTask);

            // then
            actualLabWorkflowValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
