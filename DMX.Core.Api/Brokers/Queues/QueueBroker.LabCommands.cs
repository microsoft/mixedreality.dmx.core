// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace DMX.Core.Api.Brokers.Queues
{
    public partial class QueueBroker
    {
        public IQueueClient LabCommandQueue { get; set; }

        public async ValueTask EnqueueLabCommandEventMessageAsync(Message message) =>
            await this.LabCommandQueue.SendAsync(message);
    }
}
