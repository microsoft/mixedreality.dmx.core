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
    }
}
