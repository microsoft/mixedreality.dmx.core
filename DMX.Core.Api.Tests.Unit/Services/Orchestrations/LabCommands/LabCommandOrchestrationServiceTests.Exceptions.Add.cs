// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldThrowOrchestrationDependencyValidationExceptionOnAddIfDependencyValidationErrorOccursAndLogItAsync(
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

            this.labCommandServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
