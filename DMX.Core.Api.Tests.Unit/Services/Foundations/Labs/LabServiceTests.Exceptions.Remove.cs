using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Foundations.Labs.Exceptions;
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
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            var someGuid = Guid.NewGuid();
            Lab someLab = CreateRandomLab();

            var dbUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedLabException =
                new LockedLabException(dbUpdateConcurrencyException);

            var expectedLabDependencyValidationException =
                new LabDependencyValidationException(lockedLabException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(someLab);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteLabAsync(It.IsAny<Lab>()))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<Lab> actualLabTask =
                this.labService.RemoveLabByIdAsync(someGuid);

            LabDependencyValidationException actualLabDependencyValidationException =
                await Assert.ThrowsAsync<LabDependencyValidationException>(
                    actualLabTask.AsTask);

            // then
            actualLabDependencyValidationException.Should().BeEquivalentTo(
                expectedLabDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLabAsync(It.IsAny<Lab>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            var someGuid = Guid.NewGuid();
            Lab someLab = CreateRandomLab();
            SqlException sqlException = GetSqlException();

            var failedLabStorageException =
                new FailedLabStorageException(sqlException);

            var expectedLabDependencyException =
                new LabDependencyException(failedLabStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(someLab);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteLabAsync(It.IsAny<Lab>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Lab> retrievedLabTask =
                this.labService.RemoveLabByIdAsync(someGuid);

            LabDependencyException actualLabDependencyException =
                await Assert.ThrowsAsync<LabDependencyException>(
                    retrievedLabTask.AsTask);

            // then
            actualLabDependencyException.Should().BeEquivalentTo(
                expectedLabDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLabAsync(It.IsAny<Lab>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLabDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfErrorOccursAndLogItAsync()
        {
            // given
            var someGuid = Guid.NewGuid();
            Lab someLab = CreateRandomLab();
            var exception = new Exception();

            var failedLabServiceException
                = new FailedLabServiceException(exception);

            var expectedLabServiceExceptions =
                new LabServiceException(failedLabServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(someLab);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteLabAsync(It.IsAny<Lab>()))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Lab> actualLabTask =
                this.labService.RemoveLabByIdAsync(someGuid);

            LabServiceException actualLabServiceException =
                await Assert.ThrowsAsync<LabServiceException>(
                    actualLabTask.AsTask);

            // then
            actualLabServiceException.Should().BeEquivalentTo(
                expectedLabServiceExceptions);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLabAsync(It.IsAny<Lab>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabServiceExceptions))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
