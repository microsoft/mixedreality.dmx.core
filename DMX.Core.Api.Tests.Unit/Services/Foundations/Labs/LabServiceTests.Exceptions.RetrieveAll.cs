// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
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
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedLabStorageException =
                new FailedLabStorageException(sqlException);

            var expectedLabDependencyException =
                new LabDependencyException(failedLabStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllLabs())
                    .Throws(sqlException);

            // when
            Action retrieveAllLabsAction = () =>
                this.labService.RetrieveAllLabs();

            LabDependencyException actualLabDependencyException =
                Assert.Throws<LabDependencyException>(
                    retrieveAllLabsAction);

            // then
            actualLabDependencyException.Should().BeEquivalentTo(
                expectedLabDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllLabs(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLabDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var failedLabServiceException =
                new FailedLabServiceException(exception);

            var expectedLabServiceException =
                new LabServiceException(failedLabServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllLabs())
                    .Throws(exception);

            // when
            Action retrieveAllLabsAction = () =>
                this.labService.RetrieveAllLabs();

            LabServiceException actualLabServiceExeption =
                Assert.Throws<LabServiceException>(
                    retrieveAllLabsAction);

            // then
            actualLabServiceExeption.Should().BeEquivalentTo(
                expectedLabServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllLabs(),
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
