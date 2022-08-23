// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using FluentAssertions;
using Force.DeepCloner;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Azure;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabCommandEvents
{
    public partial class LabCommandEventServiceTests
    {
        [Fact]
        public async Task ShouldAddLabCommandEventAsync()
        {
            // given
            LabCommand randomLabCommand = CreateRandomLabCommand();
            LabCommand inputLabCommand = randomLabCommand;
            LabCommand expectedLabCommand = inputLabCommand.DeepClone() ;

            string serializedLabCommand =
                JsonSerializer.Serialize(expectedLabCommand);
            
            var expectedLabCommandMessage = new Message
            {
                Body = Encoding.UTF8.GetBytes(serializedLabCommand)
            };
            
            // when
            LabCommand actualLabCommand =
                await this.labCommandEventService.AddLabCommandEventAsync(
                    inputLabCommand);

            // then
            actualLabCommand.Should().BeEquivalentTo(expectedLabCommand);

            this.queueBrokerMock.Verify(broker =>
                broker.EnqueueLabCommandEventMessageAsync(It.Is(
                    SameMessageAs(expectedLabCommandMessage))),
                        Times.Once);

            this.queueBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
