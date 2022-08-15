// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabCommands
{
    public partial class LabCommandServiceTests
    {
        [Fact]
        public async Task ShouldAddLabCommandAsync()
        {
            // given
            LabCommand randomLabCommand = CreateRandomLabCommand();
            LabCommand inputLabCommand = randomLabCommand;
            LabCommand insertedLabCommand = inputLabCommand;
            LabCommand expectedLabCommand = insertedLabCommand.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLabCommandAsync(inputLabCommand))
                    .ReturnsAsync(insertedLabCommand);

            // when
            LabCommand actualLabCommand =
                await this.labCommandService.AddLabCommandAsync(inputLabCommand);

            // then
            actualLabCommand.Should().BeEquivalentTo(expectedLabCommand);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabCommandAsync(inputLabCommand),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
