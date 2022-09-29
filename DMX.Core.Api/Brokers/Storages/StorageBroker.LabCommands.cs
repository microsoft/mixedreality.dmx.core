// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using Microsoft.EntityFrameworkCore;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<LabCommand> LabCommands { get; set; }

        public async ValueTask<LabCommand> InsertLabCommandAsync(LabCommand labCommand) =>
            await InsertAsync(labCommand);

        public async ValueTask<LabCommand> SelectLabCommandByIdAsync(Guid labCommandId) =>
            await FindAsync<LabCommand>(labCommandId);

        public async ValueTask<LabCommand> UpdateLabCommandAsync(LabCommand labCommand) =>
            await UpdateAsync(labCommand);

        public async ValueTask<LabCommand> DeleteLabCommandAsync(LabCommand labCommand) =>
            await DeleteAsync(labCommand);
    }
}