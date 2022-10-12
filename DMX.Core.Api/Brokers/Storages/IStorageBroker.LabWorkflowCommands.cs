// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<LabWorkflowCommand> InsertLabWorkflowCommandAsync(LabWorkflowCommand labWorkflowCommand);
        ValueTask<LabWorkflowCommand> SelectLabWorkflowCommandByIdAsync(Guid labWorkflowCommandId);
        ValueTask<LabWorkflowCommand> UpdateLabWorkflowCommandAsync(LabWorkflowCommand labWorkflowCommand);
        ValueTask<LabWorkflowCommand> DeleteLabWorkflowCommandAsync(LabWorkflowCommand labWorkflowCommand);
    }
}