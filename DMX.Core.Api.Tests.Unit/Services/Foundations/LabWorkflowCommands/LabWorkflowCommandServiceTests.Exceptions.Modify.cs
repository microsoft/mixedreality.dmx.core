// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
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

            this.datetimeBrokerMock.Setup(broker =>
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

            this.datetimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.datetimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
