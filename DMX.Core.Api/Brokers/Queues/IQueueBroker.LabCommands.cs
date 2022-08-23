// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;

namespace DMX.Core.Api.Brokers.Queues
{
    public partial interface IQueueBroker
    {
        public ValueTask EnqueueLabCommandEventMessageAsync(Message message); 
    }
}
