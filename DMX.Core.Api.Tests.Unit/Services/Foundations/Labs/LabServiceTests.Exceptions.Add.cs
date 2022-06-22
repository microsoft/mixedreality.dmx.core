// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.Labs
{
    public partial class LabServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRegisterIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Lab someLab = CreateRandomLab();
            SqlException sqlException = GetSqlException();

            var failedLabStorageException = new FailedLabStorageException(sqlException);
            var expectedLabDependencyException = new LabDependencyException(failedLabStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLabAsync(It.IsAny<Lab>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Lab> retrievedLabTask = this.labService.AddLabAsync(someLab);

            LabDependencyException actualLabDependencyException = await Assert.ThrowsAsync<LabDependencyException>(
                () => retrievedLabTask.AsTask());

            // then
            actualLabDependencyException.Should().BeEquivalentTo(expectedLabDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabAsync(It.IsAny<Lab>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedLabDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfLabAlreadyExistsAndLogItAsync()
        {
            // given
            Lab someLab = CreateRandomLab();
            string someMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(someMessage);

            var alreadyExistsLabException =
                new AlreadyExistsLabException(duplicateKeyException);

            var expectedLabDependencyValidationException =
                new LabDependencyValidationException(alreadyExistsLabException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLabAsync(It.IsAny<Lab>()))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Lab> addLabTask = this.labService.AddLabAsync(someLab);

            LabDependencyValidationException actualLabDependencyValidationException =
                await Assert.ThrowsAsync<LabDependencyValidationException>(addLabTask.AsTask);

            // then
            actualLabDependencyValidationException.Should()
                .BeEquivalentTo(expectedLabDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabAsync(It.IsAny<Lab>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateErrorsOccursAndLogItAsync()
        {
            // given
            Lab someLab = CreateRandomLab();

            var dbUpdateException = 
                new DbUpdateException();

            var failedLabStorageException = 
                new FailedLabStorageException(dbUpdateException);

            var expectedLabDependencyException = 
                new LabDependencyException(failedLabStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLabAsync(It.IsAny<Lab>()))
                    .ThrowsAsync(dbUpdateException);

            // when
            ValueTask<Lab> addLabTask = this.labService.AddLabAsync(someLab);

            LabDependencyException actualLabDependencyException =
                await Assert.ThrowsAsync<LabDependencyException>(addLabTask.AsTask);

            // then
            actualLabDependencyException.Should()
                .BeEquivalentTo(expectedLabDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabAsync(It.IsAny<Lab>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccurredAndLogItAsync()
        {
            // given
            Lab someLab = CreateRandomLab();

            var exception =
                new Exception();

            var failedLabServiceException =
                new FailedLabServiceException(exception);

            var expectedLabServiceException =
                new LabServiceException(failedLabServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLabAsync(It.IsAny<Lab>()))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Lab> addLabTask = this.labService.AddLabAsync(someLab);

            LabServiceException actualLabServiceException =
                await Assert.ThrowsAsync<LabServiceException>(addLabTask.AsTask);

            // then
            actualLabServiceException.Should()
                .BeEquivalentTo(expectedLabServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabAsync(It.IsAny<Lab>()),
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
