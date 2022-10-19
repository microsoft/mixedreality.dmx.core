// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;

namespace DMX.Core.Api.Services.Foundations.LabWorkflowCommands
{
    public interface ILabWorkflowCommandService
    {
        ValueTask<LabWorkflowCommand> AddLabWorkflowCommandAsync(LabWorkflowCommand labWorkflowCommand);
        ValueTask<LabWorkflowCommand> ModifyLabWorkflowCommand(LabWorkflowCommand command);
    }
}
