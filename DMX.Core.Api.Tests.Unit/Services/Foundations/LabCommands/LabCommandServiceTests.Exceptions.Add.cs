// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using DMX.Core.Api.Models.Foundations.Labs.Exceptions;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabCommands
{
    public partial class LabCommandServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            LabCommand someLabCommand = CreateRandomLabCommand();
            SqlException sqlException = GetSqlException();

            var failedLabCommandStorageException =
                new FailedLabCommandStorageException(sqlException);

            var expectedLabCommandDependencyException =
                new LabCommandDependencyException(failedLabCommandStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLabCommandAsync(It.IsAny<LabCommand>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<LabCommand> retrievedLabCommandTask =
                this.labCommandService.AddLabCommandAsync(someLabCommand);

            LabCommandDependencyException actualLabCommandDependencyException =
                await Assert.ThrowsAsync<LabCommandDependencyException>(
                    retrievedLabCommandTask.AsTask);

            // then
            actualLabCommandDependencyException.Should().BeEquivalentTo(
                expectedLabCommandDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLabCommandDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfLabCommandAlreadyExistsAndLogItAsync()
        {
            // given
            LabCommand someLabCommand = CreateRandomLabCommand();
            string someMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(someMessage);

            var alreadyExistsLabException =
                new AlreadyExistsLabCommandException(duplicateKeyException);

            var expectedLabCommandDependencyValidationException =
                new LabCommandDependencyValidationException(alreadyExistsLabException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLabCommandAsync(It.IsAny<LabCommand>()))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<LabCommand> addLabCommandTask =
                this.labCommandService.AddLabCommandAsync(someLabCommand);

            LabCommandDependencyValidationException actualLabCommandDependencyValidationException =
                await Assert.ThrowsAsync<LabCommandDependencyValidationException>(
                    addLabCommandTask.AsTask);

            // then
            actualLabCommandDependencyValidationException.Should()
                .BeEquivalentTo(expectedLabCommandDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateErrorsOccursAndLogItAsync()
        {
            // given
            LabCommand someLabCommand = CreateRandomLabCommand();

            var dbUpdateException =
                new DbUpdateException();

            var failedLabCommandStorageException =
                new FailedLabCommandStorageException(dbUpdateException);

            var expectedLabCommandDependencyException =
                new LabCommandDependencyException(failedLabCommandStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLabCommandAsync(It.IsAny<LabCommand>()))
                    .ThrowsAsync(dbUpdateException);

            // when
            ValueTask<LabCommand> addLabCommandTask =
                this.labCommandService.AddLabCommandAsync(someLabCommand);

            LabCommandDependencyException actualLabCommandDependencyException =
                await Assert.ThrowsAsync<LabCommandDependencyException>(addLabCommandTask.AsTask);

            // then
            actualLabCommandDependencyException.Should()
                .BeEquivalentTo(expectedLabCommandDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
