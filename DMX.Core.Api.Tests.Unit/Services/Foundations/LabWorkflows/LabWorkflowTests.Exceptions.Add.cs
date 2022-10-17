// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflows
{
    public partial class LabWorkflowTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            LabWorkflow randomLabWorkflow = CreateRandomLabWorkflow(dateTime);
            LabWorkflow inputLabWorkflow = randomLabWorkflow;

            var sqlException = GetSqlException();

            var failedLabWorkflowStorageException =
                new FailedLabWorkflowStorageException(sqlException);

            var expectedLabWorkflowDependencyException =
                new LabWorkflowDependencyException(failedLabWorkflowStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLabWorkflowAsync(inputLabWorkflow))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<LabWorkflow> addLabWorkflowTask =
                this.labWorkflowService.AddLabWorkflowAsync(inputLabWorkflow);

            // then
            await Assert.ThrowsAsync<LabWorkflowDependencyException>(() =>
                addLabWorkflowTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedLabWorkflowDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabWorkflowAsync(inputLabWorkflow),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
