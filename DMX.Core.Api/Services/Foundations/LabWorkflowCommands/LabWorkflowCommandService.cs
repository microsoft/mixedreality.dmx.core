// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Storages;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;

namespace DMX.Core.Api.Services.Foundations.LabWorkflowCommands
{
    public class LabWorkflowCommandService : ILabWorkflowCommandService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public LabWorkflowCommandService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<LabWorkflowCommand> AddLabWorkflowCommandAsync(LabWorkflowCommand labWorkflowCommand)
        {
            throw new System.NotImplementedException();
        }
    }
}
