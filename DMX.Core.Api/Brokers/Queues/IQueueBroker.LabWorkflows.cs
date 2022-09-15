// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace DMX.Core.Api.Brokers.Queues
{
    public partial interface IQueueBroker
    {
        public ValueTask EnqueueLabWorkflowEventMessageAsync(Message message);
    }
}