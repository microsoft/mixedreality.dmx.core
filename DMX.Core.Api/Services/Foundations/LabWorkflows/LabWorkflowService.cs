// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Brokers.DateTimes;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Storages;
using DMX.Core.Api.Models.Foundations.LabWorkflows;

namespace DMX.Core.Api.Services.Foundations.LabWorkflows
{
    public partial class LabWorkflowService : ILabWorkflowService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public LabWorkflowService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<LabWorkflow> AddLabWorkflowAsync(LabWorkflow labWorkflow) =>
        TryCatch(async () =>
        {
            ValidateLabWorkflowOnAdd(labWorkflow);

            return await this.storageBroker.InsertLabWorkflowAsync(labWorkflow);
        });

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
