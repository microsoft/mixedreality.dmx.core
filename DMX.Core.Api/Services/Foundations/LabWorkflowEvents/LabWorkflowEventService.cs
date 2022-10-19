// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Queues;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using Microsoft.Azure.ServiceBus;

namespace DMX.Core.Api.Services.Foundations.LabWorkflowEvents
{
    public partial class LabWorkflowEventService : ILabWorkflowEventService
    {
        private IQueueBroker queueBroker;
        private ILoggingBroker loggingBroker;

        public LabWorkflowEventService(
            IQueueBroker queueBroker,
            ILoggingBroker loggingBroker)
        {
            this.queueBroker = queueBroker;
            this.loggingBroker = loggingBroker;
        }
        public ValueTask<LabWorkflow> AddLabWorkflowEventAsync(LabWorkflow labWorkflow) =>
        TryCatch(async () =>
        {
            ValidateIfNull(labWorkflow);
            Message labWorkflowMessage = MapToMessage(labWorkflow);
            await this.queueBroker.EnqueueLabWorkflowEventMessageAsync(labWorkflowMessage);

            return labWorkflow;
        });

        private Message MapToMessage(LabWorkflow labWorkflow)
        {
            string serializedLabWorkflowEvent =
                JsonSerializer.Serialize(labWorkflow);

            return new Message
            {
                Body = Encoding.UTF8.GetBytes(serializedLabWorkflowEvent),
            };
        }
    }
}
