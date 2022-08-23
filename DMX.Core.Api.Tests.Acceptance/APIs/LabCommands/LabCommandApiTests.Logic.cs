// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using FluentAssertions;
using Force.DeepCloner;
using Xunit;

namespace DMX.Core.Api.Tests.Acceptance.APIs.LabCommands
{
    public partial class LabCommandApiTests
    {
        [Fact]
        public async Task ShouldPostLabCommandAsync()
        {
            // given
            LabCommand randomLabCommand = CreateRandomLabCommand();
            var inputLabCommand = randomLabCommand;
            var expectedLabCommand = randomLabCommand.DeepClone();

            // when
            LabCommand actualLabCommand =
                await this.dmxCoreApiBroker.PostLabCommandAsync(inputLabCommand);

            // then
            actualLabCommand.Should().BeEquivalentTo(expectedLabCommand);
        }

        [Fact]
        public async Task ShouldRetrieveByIdLabCommandAsync()
        {
            // given
            LabCommand randomLabCommand = CreateRandomLabCommand();
            var inputLabCommand = randomLabCommand;
            var inputLabCommandId = inputLabCommand.Id;
            var expectedLabCommand = randomLabCommand.DeepClone();

            // when
            await this.dmxCoreApiBroker.PostLabCommandAsync(inputLabCommand);

            LabCommand actualLabCommand =
                await this.dmxCoreApiBroker.GetLabCommandByIdAsync(inputLabCommandId);

            // then
            actualLabCommand.Should().BeEquivalentTo(expectedLabCommand);
        }

        [Fact]
        public async Task ShouldPutLabCommandAsync()
        {
            // given
            LabCommand randomLabCommand = CreateRandomLabCommand();
            var inputLabCommand = randomLabCommand;
            var expectedLabCommand = randomLabCommand.DeepClone();

            // when
            await this.dmxCoreApiBroker.PutLabCommandAsync(inputLabCommand);

            LabCommand actualLabCommand =
                await this.dmxCoreApiBroker.PutLabCommandAsync(inputLabCommand);

            // then
            actualLabCommand.Should().BeEquivalentTo(expectedLabCommand);
        }
    }
}
