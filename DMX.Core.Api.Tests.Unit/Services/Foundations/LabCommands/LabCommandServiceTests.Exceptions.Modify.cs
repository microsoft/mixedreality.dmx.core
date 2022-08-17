// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using FluentAssertions;
using Force.DeepCloner;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabCommands
{
    public partial class LabCommandServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            LabCommand someLabCommand = CreateRandomLabCommand(randomDateTimeOffset);
            int randomNumber = GetRandomNumber();
            someLabCommand.UpdatedDate = someLabCommand.CreatedDate.AddSeconds(randomNumber);
            SqlException sqlException = GetSqlException();

            var failedLabCommandStorageException =
                new FailedLabCommandStorageException(sqlException);

            var expectedLabCommandDependencyException =
                new LabCommandDependencyException(failedLabCommandStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

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
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOAddIfDbUpdateErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            LabCommand randomLabCommand = CreateRandomLabCommand(randomDateTimeOffset);
            LabCommand storageLabCommand = randomLabCommand.DeepClone();
            LabCommand inputLabCommand = randomLabCommand;
            int randomNumber = GetRandomNumber();
            inputLabCommand.UpdatedDate = inputLabCommand.CreatedDate.AddSeconds(randomNumber);
            var dbUpdateException = new DbUpdateException();

            var failedLabCommandStorageException =
                new FailedLabCommandStorageException(dbUpdateException);

            var expectedLabCommandDependencyException =
                new LabCommandDependencyException(failedLabCommandStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageLabCommand);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateLabCommandAsync(It.IsAny<LabCommand>()))
                    .ThrowsAsync(dbUpdateException);

            // when
            ValueTask<LabCommand> modifyLabCommandTask =
                this.labCommandService.ModifyLabCommandAsync(inputLabCommand);

            LabCommandDependencyException actualLabCommandDependencyException =
                await Assert.ThrowsAsync<LabCommandDependencyException>(
                    modifyLabCommandTask.AsTask);

            // then
            actualLabCommandDependencyException.Should().BeEquivalentTo(
                expectedLabCommandDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            LabCommand randomLabCommand = CreateRandomLabCommand(randomDateTimeOffset);
            LabCommand inputLabCommand = randomLabCommand;
            int randomNumber = GetRandomNumber();
            inputLabCommand.UpdatedDate = inputLabCommand.CreatedDate.AddSeconds(randomNumber);
            var serviceException = new Exception();

            var failedLabCommandServiceException =
                new FailedLabCommandServiceException(serviceException);

            var expectedLabCommandServiceException =
                new LabCommandServiceException(failedLabCommandServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<LabCommand> modifyLabCommandTask =
                this.labCommandService.ModifyLabCommandAsync(inputLabCommand);

            LabCommandServiceException actualLabCommandServiceException =
                await Assert.ThrowsAsync<LabCommandServiceException>(
                    modifyLabCommandTask.AsTask);

            // then
            actualLabCommandServiceException.Should().BeEquivalentTo(
                expectedLabCommandServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
