// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Foundations.Labs.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.Labs
{
    public partial class LabServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someLabId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedLabStorageException =
                new FailedLabStorageException(sqlException);

            var expectedLabDependencyException =
                new LabDependencyException(failedLabStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabByIdAsync(someLabId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Lab> retrieveByIdTask =
                this.labService.RetrieveLabByIdAsync(someLabId);

            LabDependencyException actualLabDependencyException =
                await Assert.ThrowsAsync<LabDependencyException>(
                    retrieveByIdTask.AsTask);

            //then
            actualLabDependencyException.Should().BeEquivalentTo(
                expectedLabDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLabDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someLabId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedLabServiceException =
                new FailedLabServiceException(serviceException);

            var expectedLabServiceException =
                new LabServiceException(failedLabServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabByIdAsync(someLabId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Lab> retrieveByIdTask =
                this.labService.RetrieveLabByIdAsync(someLabId);

            LabServiceException actualLabServiceException =
                await Assert.ThrowsAsync<LabServiceException>(
                    retrieveByIdTask.AsTask);

            //then
            actualLabServiceException.Should().BeEquivalentTo(
                expectedLabServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
