// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldRetrieveLabCommandByIdAsync()
        {
            // given
            Guid randomLabCommandId = Guid.NewGuid();
            Guid inputLabCommandId = randomLabCommandId;
            LabCommand randomLabCommand = CreateRandomLabCommand();
            LabCommand selectedLabCommand = randomLabCommand;
            LabCommand expectedLabCommand = selectedLabCommand.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabCommandByIdAsync(randomLabCommandId))
                    .ReturnsAsync(selectedLabCommand);

            // when
            LabCommand actualLabCommand =
                await this.labCommandService.RetrieveLabCommandByIdAsync(inputLabCommandId);

            // then
            actualLabCommand.Should().BeEquivalentTo(expectedLabCommand);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabCommandByIdAsync(inputLabCommandId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
