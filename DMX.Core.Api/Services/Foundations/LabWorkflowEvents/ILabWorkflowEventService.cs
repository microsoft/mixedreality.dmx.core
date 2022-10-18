// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;

namespace DMX.Core.Api.Services.Foundations.LabWorkflowEvents
{
    public interface ILabWorkflowEventService
    {
        ValueTask<LabWorkflow> AddLabWorkflowEventAsync(LabWorkflow labWorkflow);
    }
}
