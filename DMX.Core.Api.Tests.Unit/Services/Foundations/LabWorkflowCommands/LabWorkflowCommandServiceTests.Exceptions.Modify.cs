// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            LabWorkflowCommand someLabWorkflowCommand = CreateRandomLabWorkflowCommand();
            SqlException sqlException = GetSqlException();

            var failedLabWorkflowCommandStorageException =
                new FailedLabWorkflowCommandStorageException(sqlException);

            var expectedLabWorkflowCommandDependencyException =
                new LabWorkflowCommandDependencyException(failedLabWorkflowCommandStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<LabWorkflowCommand> modifyLabCommandTask =
                this.labWorkflowCommandService.ModifyLabWorkflowCommand(someLabWorkflowCommand);

            LabWorkflowCommandDependencyException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandDependencyException>(
                    modifyLabCommandTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            LabWorkflowCommand someLabWorkflowCommand = CreateRandomLabWorkflowCommand();
            DbUpdateException dbUpdateException = new DbUpdateException();

            var failedLabWorkflowCommandStorageException =
                new FailedLabWorkflowCommandStorageException(dbUpdateException);

            var expectedLabWorkflowCommandDependencyException =
                new LabWorkflowCommandDependencyException(failedLabWorkflowCommandStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(dbUpdateException);

            // when
            ValueTask<LabWorkflowCommand> modifyLabCommandTask =
                this.labWorkflowCommandService.ModifyLabWorkflowCommand(someLabWorkflowCommand);

            LabWorkflowCommandDependencyException actualLabWorkflowCommandValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandDependencyException>(
                    modifyLabCommandTask.AsTask);

            // then
            actualLabWorkflowCommandValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task 
            ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            LabWorkflowCommand randomLabWorkflowCommand = CreateRandomLabWorkflowCommand();
            LabWorkflowCommand someLabWorkflowCommand = randomLabWorkflowCommand;
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedLabWorkflowCommandException =
                new LockedLabWorkflowCommandException(dbUpdateConcurrencyException);

            var expectedLabWorkflowCommandDependencyValidationException = 
                new LabWorkflowCommandDependencyValidationException(lockedLabWorkflowCommandException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(dbUpdateConcurrencyException);

            // when
            ValueTask<LabWorkflowCommand> modifyLabWorkflowCommandTask =
                this.labWorkflowCommandService.ModifyLabWorkflowCommand(someLabWorkflowCommand);

            LabWorkflowCommandDependencyValidationException actualLabWorkflowCommandDependencyValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandDependencyValidationException>(
                    modifyLabWorkflowCommandTask.AsTask);

            // then
            actualLabWorkflowCommandDependencyValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
