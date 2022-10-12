// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;

namespace DMX.Core.Api.Services.Foundations.LabWorkflows
{
    public interface ILabWorkflowService
    {
        ValueTask<LabWorkflow> RetrieveLabWorkflowByIdAsync(Guid labWorkflowId);
        ValueTask<LabWorkflow> AddLabWorkflowAsync(LabWorkflow labWorkflow);
    }
}
