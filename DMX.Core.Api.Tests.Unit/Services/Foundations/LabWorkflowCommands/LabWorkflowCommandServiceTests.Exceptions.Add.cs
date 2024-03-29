﻿// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfErrorOcccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            LabWorkflowCommand randomLabWorkflowCommand = CreateRandomLabWorkflowCommand(dateTime);
            LabWorkflowCommand inputLabWorkflowCommand = randomLabWorkflowCommand;

            var sqlException = GetSqlException();

            var failedLabWorkflowCommandStorageException =
                new FailedLabWorkflowCommandStorageException(sqlException);

            var expectedLabWorkflowCommandDependencyException =
                new LabWorkflowCommandDependencyException(failedLabWorkflowCommandStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLabWorkflowCommandAsync(inputLabWorkflowCommand))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<LabWorkflowCommand> addLabWorkflowCommandTask =
                this.labWorkflowCommandService.AddLabWorkflowCommandAsync(
                    inputLabWorkflowCommand);

            LabWorkflowCommandDependencyException actualLabWorkflowCommandDependencyException =
                await Assert.ThrowsAsync<LabWorkflowCommandDependencyException>(
                    addLabWorkflowCommandTask.AsTask);
            // then
            actualLabWorkflowCommandDependencyException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedLabWorkflowCommandDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabWorkflowCommandAsync(
                    It.IsAny<LabWorkflowCommand>()),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfLabWorkflowCommandAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            LabWorkflowCommand randomLabWorkflowCommand = CreateRandomLabWorkflowCommand(dateTime);
            string someMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(someMessage);

            var alreadyExistsLabWorkflowCommandException =
                new AlreadyExistsLabWorkflowCommandException(duplicateKeyException);

            var expectedLabWorkflowCommandDependencyValidationException =
                new LabWorkflowCommandDependencyValidationException(alreadyExistsLabWorkflowCommandException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<LabWorkflowCommand> addLabWorkflowCommandTask =
                this.labWorkflowCommandService.AddLabWorkflowCommandAsync(
                    randomLabWorkflowCommand);

            LabWorkflowCommandDependencyValidationException actualLabWorkflowCommandDependencyValidationException =
                await Assert.ThrowsAsync<LabWorkflowCommandDependencyValidationException>(
                    addLabWorkflowCommandTask.AsTask);
            // then
            actualLabWorkflowCommandDependencyValidationException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandDependencyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabWorkflowCommandAsync(
                    It.IsAny<LabWorkflowCommand>()),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            LabWorkflowCommand randomLabWorkflowCommand = CreateRandomLabWorkflowCommand(dateTime);

            var dbUpdateException =
                new DbUpdateException();

            var failedLabWorkflowCommandStorageException =
                new FailedLabWorkflowCommandStorageException(dbUpdateException);

            var expectedLabWorkflowCommandDependencyException =
                new LabWorkflowCommandDependencyException(failedLabWorkflowCommandStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLabWorkflowCommandAsync(It.IsAny<LabWorkflowCommand>()))
                    .ThrowsAsync(dbUpdateException);

            // when
            ValueTask<LabWorkflowCommand> addLabWorkflowCommandTask =
                this.labWorkflowCommandService.AddLabWorkflowCommandAsync(
                    randomLabWorkflowCommand);

            LabWorkflowCommandDependencyException actualLabWorkflowCommandDependencyException =
                await Assert.ThrowsAsync<LabWorkflowCommandDependencyException>(
                    addLabWorkflowCommandTask.AsTask);
            // then
            actualLabWorkflowCommandDependencyException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabWorkflowCommandAsync(
                    It.IsAny<LabWorkflowCommand>()),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            LabWorkflowCommand randomLabWorkflowCommand = CreateRandomLabWorkflowCommand(dateTime);
            var serviceException = new Exception();

            var failedLabWorkflowCommandServiceException =
                new FailedLabWorkflowCommandServiceException(serviceException);

            var expectedLabWorkflowCommandServiceException =
                new LabWorkflowCommandServiceException(failedLabWorkflowCommandServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLabWorkflowCommandAsync(
                    It.IsAny<LabWorkflowCommand>()))
                        .ThrowsAsync(serviceException);

            // when
            ValueTask<LabWorkflowCommand> addLabWorkflowCommandTask =
                this.labWorkflowCommandService.AddLabWorkflowCommandAsync(
                    randomLabWorkflowCommand);

            LabWorkflowCommandServiceException actualLabWorkflowCommandServiceException =
                await Assert.ThrowsAsync<LabWorkflowCommandServiceException>(
                    addLabWorkflowCommandTask.AsTask);
            // then
            actualLabWorkflowCommandServiceException.Should().BeEquivalentTo(
                expectedLabWorkflowCommandServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabWorkflowCommandServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabWorkflowCommandAsync(
                    It.IsAny<LabWorkflowCommand>()),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
