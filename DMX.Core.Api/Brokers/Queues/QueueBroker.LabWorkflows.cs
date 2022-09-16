// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace DMX.Core.Api.Brokers.Queues
{
    public partial class QueueBroker
    {
        public IQueueClient LabWorkflowQueue { get; set; }

        public async ValueTask EnqueueLabWorkflowEventMessageAsync(Message message) =>
            await this.LabWorkflowQueue.SendAsync(message);
    }
}