// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Orchestrations.LabCommands.Exceptions;
using FluentAssertions;
using Moq;
using Xeptions;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations.LabCommands
{
    public partial class LabCommandOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(LabCommandDependencyValidationExceptions))]
        public async Task
            ShouldThrowOrchestrationDependencyValidationExceptionOnAddIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption validationException)
        {
            // given
            LabCommand randomLabCommand = CreateRandomLabCommand();
            LabCommand someLabCommand = randomLabCommand;

            var expectedLabCommandOrchestrationDependencyValidationException =
                new LabCommandOrchestrationDependencyValidationException(
                    validationException.InnerException as Xeption);

            this.labCommandServiceMock.Setup(service =>
                service.AddLabCommandAsync(It.IsAny<LabCommand>()))
                    .ThrowsAsync(validationException);

            // when
            ValueTask<LabCommand> actualLabTask =
                this.labCommandOrchestrationService.AddLabCommandAsync(someLabCommand);

            LabCommandOrchestrationDependencyValidationException
                actualLabCommandOrchestrationDependencyValidationException =
                    await Assert.ThrowsAsync<LabCommandOrchestrationDependencyValidationException>(
                        actualLabTask.AsTask);

            // then
            actualLabCommandOrchestrationDependencyValidationException.Should()
                .BeEquivalentTo(expectedLabCommandOrchestrationDependencyValidationException);

            this.labCommandServiceMock.Verify(service =>
                service.AddLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandOrchestrationDependencyValidationException))),
                        Times.Once);

            this.labCommandEventServiceMock.Verify(service =>
                service.AddLabCommandEventAsync(It.IsAny<LabCommand>()),
                    Times.Never);

            this.labCommandServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.labCommandEventServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(LabCommandDependencyExceptions))]
        public async Task ShouldThrowOrchestrationDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            LabCommand randomLabCommand = CreateRandomLabCommand();
            LabCommand someLabCommand = randomLabCommand;

            var expectedLabCommandOrchestrationDependencyException =
                new LabCommandOrchestrationDependencyException(
                    dependencyException.InnerException as Xeption);

            this.labCommandEventServiceMock.Setup(service =>
                service.AddLabCommandEventAsync(It.IsAny<LabCommand>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<LabCommand> addLabCommandTask =
                this.labCommandOrchestrationService.AddLabCommandAsync(someLabCommand);

            LabCommandOrchestrationDependencyException actualLabCommandOrchestrationDependencyException =
                await Assert.ThrowsAsync<LabCommandOrchestrationDependencyException>(
                    addLabCommandTask.AsTask);

            // then
            actualLabCommandOrchestrationDependencyException.Should().BeEquivalentTo(
                expectedLabCommandOrchestrationDependencyException);

            this.labCommandEventServiceMock.Verify(service =>
                service.AddLabCommandEventAsync(It.IsAny<LabCommand>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandOrchestrationDependencyException))),
                        Times.Once);

            this.labCommandServiceMock.Verify(service =>
                service.AddLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Never);

            this.labCommandEventServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.labCommandServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOrchestrationServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            LabCommand randomLabCommand = CreateRandomLabCommand();
            var exception = new Exception();

            var failedLabCommandOrchestrationServiceException =
                new FailedLabCommandOrchestrationServiceException(exception);

            var expectedLabCommandOrchestrationServiceException =
                new LabCommandOrchestrationServiceException(
                    failedLabCommandOrchestrationServiceException);

            this.labCommandServiceMock.Setup(service =>
                service.AddLabCommandAsync(It.IsAny<LabCommand>()))
                    .ThrowsAsync(exception);

            // when
            ValueTask<LabCommand> addLabCommandTask =
                this.labCommandOrchestrationService.AddLabCommandAsync(randomLabCommand);

            LabCommandOrchestrationServiceException
                actualLabCommandOrchestrationServiceException =
                    await Assert.ThrowsAsync<LabCommandOrchestrationServiceException>(
                        addLabCommandTask.AsTask);

            // then
            actualLabCommandOrchestrationServiceException.Should().BeEquivalentTo(
                expectedLabCommandOrchestrationServiceException);

            this.labCommandServiceMock.Verify(service =>
                service.AddLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabCommandOrchestrationServiceException))),
                        Times.Once);

            this.labCommandServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
