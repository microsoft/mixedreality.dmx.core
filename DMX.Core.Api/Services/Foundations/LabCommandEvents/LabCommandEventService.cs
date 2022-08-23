// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Queues;
using DMX.Core.Api.Models.Foundations.LabCommandEvents;

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

        public ValueTask<LabCommandEvent> AddLabCommandEventAsync(
            LabCommandEvent labCommandEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}
