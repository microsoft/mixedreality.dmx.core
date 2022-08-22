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
            LabCommand returnedLabCommand = inputLabCommand.DeepClone();
            LabCommand expectedLabCommand = inputLabCommand.DeepClone();

            this.labCommandServiceMock.Setup(broker =>
                broker.AddLabCommandAsync(inputLabCommand))
                    .ReturnsAsync(returnedLabCommand);

            // when
            LabCommand actualLabCommand =
                await this.labCommandOrchestrationService.AddLabCommandAsync(
                    inputLabCommand);

            // then
            actualLabCommand.Should().BeEquivalentTo(expectedLabCommand);

            this.labCommandServiceMock.Verify(broker =>
                broker.AddLabCommandAsync(inputLabCommand),
                    Times.Once());

            this.labCommandServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
