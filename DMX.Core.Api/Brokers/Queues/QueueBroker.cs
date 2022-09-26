// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace DMX.Core.Api.Brokers.Queues
{
    public partial class QueueBroker : IQueueBroker
    {
        private readonly IConfiguration configuration;

        public QueueBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            InitializeClients();
        }

        private void InitializeClients()
        {
            this.LabCommandQueue = GetQueueClient(nameof(LabCommandQueue));
            this.LabWorkflowQueue = GetQueueClient(nameof(LabWorkflowQueue));
        }

        private IQueueClient GetQueueClient(string queueName)
        {
            string queueConnectionString =
                this.configuration.GetConnectionString("ServiceBusConnection");

            return new QueueClient(queueConnectionString, queueName);
        }
    }
}
