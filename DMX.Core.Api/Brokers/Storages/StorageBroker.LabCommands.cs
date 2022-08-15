// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<LabCommand> LabCommands { get; set; }

        public async ValueTask<LabCommand> InsertLabCommand(LabCommand command)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<LabCommand> labCommandEntityEntry =
                await broker.LabCommands.AddAsync(command);

            await broker.SaveChangesAsync();

            return labCommandEntityEntry.Entity;
        }
    }
}
