// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations.LabCommands
{
    public partial class LabCommandOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldAddLabCommandAsync()
        {
            // given
            LabCommand randomLabCommand = CreateRandomLabCommand();
            LabCommand inputLabCommand = randomLabCommand;
            LabCommand eventLabCommand = inputLabCommand;
            LabCommand returnedLabCommand = inputLabCommand;
            LabCommand expectedLabCommand = returnedLabCommand.DeepClone();
            var mockSequence = new MockSequence();

            this.labCommandServiceMock.InSequence(mockSequence).Setup(broker =>
                broker.AddLabCommandAsync(inputLabCommand))
                    .ReturnsAsync(returnedLabCommand);

            this.labCommandEventServiceMock.InSequence(mockSequence).Setup(broker =>
                broker.AddLabCommandEventAsync(inputLabCommand))
                    .ReturnsAsync(eventLabCommand);

            // when
            LabCommand actualLabCommand =
                await this.labCommandOrchestrationService.AddLabCommandAsync(
                    inputLabCommand);

            // then
            actualLabCommand.Should().BeEquivalentTo(expectedLabCommand);

            this.labCommandServiceMock.Verify(broker =>
                broker.AddLabCommandAsync(inputLabCommand),
                    Times.Once());

            this.labCommandEventServiceMock.Verify(broker =>
                broker.AddLabCommandEventAsync(inputLabCommand),
                    Times.Once());

            this.labCommandServiceMock.VerifyNoOtherCalls();
            this.labCommandEventServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
