// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            LabCommand someLabCommand = CreateRandomLabCommand();
            int randomNumber = GetRandomNumber();
            someLabCommand.UpdatedDate = someLabCommand.CreatedDate.AddSeconds(randomNumber);
            SqlException sqlException = GetSqlException();

            var failedLabCommandStorageException =
                new FailedLabCommandStorageException(sqlException);

            var expectedLabCommandDependencyException = 
                new LabCommandDependencyException(failedLabCommandStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<LabCommand> modifyLabCommandTask =
                this.labCommandService.ModifyLabCommandAsync(someLabCommand);

            LabCommandDependencyException actualLabCommandDependencyException =
                await Assert.ThrowsAsync<LabCommandDependencyException>(
                    modifyLabCommandTask.AsTask);

            // then
            actualLabCommandDependencyException.Should().BeEquivalentTo(
                expectedLabCommandDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLabCommandDependencyException))),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Once());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
