// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RESTFulSense.Models;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflows
{
    public partial class LabWorkflowTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlExceptionOccurrsAndLogItAsync()
        {
            // given
            Guid someLabWorkflowId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedLabWorkflowStorageException =
                new FailedLabWorkflowStorageException(sqlException);

            var expectedLabWorkflowDependencyException =
                new LabWorkflowDependencyException(failedLabWorkflowStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabWorkflowByIdAsync(someLabWorkflowId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<LabWorkflow> retrieveByIdTask =
                this.labWorkflowService.RetrieveLabWorkflowByIdAsync(someLabWorkflowId);

            LabWorkflowDependencyException actualLabWorkflowDependencyException =
                await Assert.ThrowsAsync<LabWorkflowDependencyException>(
                    retrieveByIdTask.AsTask);

            // then
            actualLabWorkflowDependencyException.Should().BeEquivalentTo(
                expectedLabWorkflowDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLabWorkflowDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someLabWorkflowId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedLabWorkflowServiceException =
                new FailedLabWorkflowServiceException(serviceException);

            var expectedLabWorkflowServiceException =
                new LabWorkflowServiceException(failedLabWorkflowServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabWorkflowByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<LabWorkflow> retrievedLabWorkflowTask =
                this.labWorkflowService.RetrieveLabWorkflowByIdAsync(someLabWorkflowId);

            LabWorkflowServiceException actualLabWorkflowServiceException =
                await Assert.ThrowsAsync<LabWorkflowServiceException>(
                    retrievedLabWorkflowTask.AsTask);

            // then
            actualLabWorkflowServiceException.Should().BeEquivalentTo(
                expectedLabWorkflowServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
