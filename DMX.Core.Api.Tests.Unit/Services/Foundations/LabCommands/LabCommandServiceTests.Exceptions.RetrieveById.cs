// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using DMX.Core.Api.Models.Foundations.Labs.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
