// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using Microsoft.EntityFrameworkCore;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<LabWorkflowCommand> LabWorkflowCommands { get; set; }

        public async ValueTask<LabWorkflowCommand> InsertLabWorkflowCommandAsync(LabWorkflowCommand labWorkflowCommand) =>
            await InsertAsync(labWorkflowCommand);

        public async ValueTask<LabWorkflowCommand> SelectLabWorkflowCommandByIdAsync(Guid labWorkflowCommandId) =>
            await FindAsync<LabWorkflowCommand>(labWorkflowCommandId);

        public async ValueTask<LabWorkflowCommand> UpdateLabWorkflowCommandAsync(LabWorkflowCommand labWorkflowCommand) =>
            await UpdateAsync(labWorkflowCommand);

        public async ValueTask<LabWorkflowCommand> DeleteLabWorkflowCommandAsync(LabWorkflowCommand labWorkflowCommand) =>
            await DeleteAsync(labWorkflowCommand);
    }
}