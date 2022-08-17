// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabCommands
{
    public partial class LabCommandServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someLabCommandId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedLabStorageException =
                new FailedLabCommandStorageException(sqlException);

            var expectectedLabCommandDependencyException =
                new LabCommandDependencyException(failedLabStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabCommandByIdAsync(someLabCommandId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<LabCommand> retrieveByIdTask =
                this.labCommandService.RetrieveLabCommandByIdAsync(someLabCommandId);

            LabCommandDependencyException actualLabCommandDependencyException =
                await Assert.ThrowsAsync<LabCommandDependencyException>(
                    retrieveByIdTask.AsTask);

            // then
            actualLabCommandDependencyException.Should().BeEquivalentTo(
                expectectedLabCommandDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectectedLabCommandDependencyException))),
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

            var failedLabCommandServiceException =
                new FailedLabCommandServiceException(serviceException);

            var expectedLabCommandServiceException =
                new LabCommandServiceException(failedLabCommandServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<LabCommand> retrievedLabCommandTask =
                this.labCommandService.RetrieveLabCommandByIdAsync(someLabId);

            LabCommandServiceException actualLabCommandServiceException =
                await Assert.ThrowsAsync<LabCommandServiceException>(
                    retrievedLabCommandTask.AsTask);

            // then
            actualLabCommandServiceException.Should().BeEquivalentTo(
                expectedLabCommandServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
