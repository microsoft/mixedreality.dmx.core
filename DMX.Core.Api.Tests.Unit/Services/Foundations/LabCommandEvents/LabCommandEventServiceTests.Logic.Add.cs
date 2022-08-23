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
using DMX.Core.Api.Models.Foundations.LabCommandEvents;
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
            LabCommandEvent randomLabCommandEvent = CreateRandomLabCommandEvent();
            LabCommandEvent inputLabCommandEvent = randomLabCommandEvent;
            LabCommandEvent expectedLabCommandEvent = inputLabCommandEvent.DeepClone() ;

            string serializedLabCommandEvent =
                JsonSerializer.Serialize(expectedLabCommandEvent);
            
            var expectedLabCommandEventMessage = new Message
            {
                Body = Encoding.UTF8.GetBytes(serializedLabCommandEvent)
            };
            
            // when
            LabCommandEvent actualLabCommandEvent =
                await this.labCommandEventService.AddLabCommandEventAsync(
                    inputLabCommandEvent);

            // then
            actualLabCommandEvent.Should().BeEquivalentTo(expectedLabCommandEvent);

            this.queueBrokerMock.Verify(broker =>
                broker.EnqueueLabCommandEventMessageAsync(It.Is(
                    SameMessageAs(expectedLabCommandEventMessage))),
                        Times.Once);

            this.queueBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
