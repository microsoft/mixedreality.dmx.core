// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Storages;
using DMX.Core.Api.Models.Foundations.LabWorkflows;

namespace DMX.Core.Api.Services.Foundations.LabWorkflows
{
    public partial class LabWorkflowService : ILabWorkflowService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public LabWorkflowService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<LabWorkflow> RetrieveLabWorkflowByIdAsync(Guid labWorkflowId) =>
        TryCatch(async () =>
        {
            ValidateLabWorkflowId(labWorkflowId);

            LabWorkflow maybeLabWorkflow =
                await this.storageBroker.SelectLabWorkflowByIdAsync(labWorkflowId);

            ValidateLabWorkflowExists(maybeLabWorkflow, labWorkflowId);

            return maybeLabWorkflow;
        });
    }
}
