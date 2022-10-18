// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Queues;
using DMX.Core.Api.Models.Foundations.LabWorkflows;

namespace DMX.Core.Api.Services.Foundations.LabWorkflowEvents
{
    public class LabWorkflowEventService : ILabWorkflowEventService
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
        public ValueTask<LabWorkflow> AddLabWorkflowEventAsync(LabWorkflow labWorkflow)
        {
            throw new System.NotImplementedException();
        }
    }
}
