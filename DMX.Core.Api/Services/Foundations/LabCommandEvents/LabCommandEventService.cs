// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Queues;
using DMX.Core.Api.Models.Foundations.LabCommands;
using Microsoft.Azure.ServiceBus;

namespace DMX.Core.Api.Services.Foundations.LabCommandEvents
{
    public partial class LabCommandEventService : ILabCommandEventService
    {
        private IQueueBroker queueBroker;
        private ILoggingBroker loggingBroker;

        public LabCommandEventService(
            IQueueBroker queueBroker,
            ILoggingBroker loggingBroker)
        {
            this.queueBroker = queueBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<LabCommand> AddLabCommandEventAsync(LabCommand labCommand) =>
        TryCatch(async () =>
        {
            ValidateLabCommandIsNotNull(labCommand);
            Message labCommandMessage = MapToMessage(labCommand);
            await this.queueBroker.EnqueueLabCommandEventMessageAsync(labCommandMessage);

            return labCommand;
        });

        private Message MapToMessage(LabCommand labCommand)
        {
            string serializedLabCommandEvent =
                JsonSerializer.Serialize(labCommand);

            return new Message
            {
                Body = Encoding.UTF8.GetBytes(serializedLabCommandEvent),
            };
        }
    }
}
