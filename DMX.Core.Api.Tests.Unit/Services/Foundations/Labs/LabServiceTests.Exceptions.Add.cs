// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
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
    }
}
