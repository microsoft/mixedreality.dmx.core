// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabWorkflows;
using System.Threading.Tasks;
using System;

namespace DMX.Core.Api.Services.Orchestrations.LabWorkflows
{
    public interface ILabWorkflowOrchestrationService
    {
        ValueTask<LabWorkflow> RetrieveLabWorkflowByIdAsync(Guid labWorkflowId);
    }
}
