// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<LabWorkflow> InsertLabWorkflowAsync(LabWorkflow labWorkflow);
        ValueTask<LabWorkflow> SelectLabWorkflowByIdAsync(Guid labWorkflowId);
    }
}