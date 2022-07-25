using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            var someGuid = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedLabStorageException =
                new FailedLabStorageException(sqlException);

            var expectedLabDependencyException =
                new LabDependencyException(failedLabStorageException);

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
    }
}
