// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using DMX.Core.Api.Models.Foundations.LabWorkflows;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<LabWorkflow> LabWorkflows { get; set; }

        public async ValueTask<LabWorkflow> InsertLabWorkflowAsync(LabWorkflow workflow)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<LabWorkflow> labWorkflowEntityEntry =
                await broker.LabWorkflows.AddAsync(workflow);

            await broker.SaveChangesAsync();

            return labWorkflowEntityEntry.Entity;
        }

        public async ValueTask<LabWorkflow> SelectLabWorkflowByIdAsync(Guid labWorkflowId)
        {
            var broker = new StorageBroker(this.configuration);

            return await broker.LabWorkflows.FindAsync(labWorkflowId);
        }

        public async ValueTask<LabWorkflow> UpdateLabWorkflowAsync(LabWorkflow workflow)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<LabWorkflow> labWorkflowEntityEntry =
                broker.LabWorkflows.Update(workflow);

            await broker.SaveChangesAsync();

            return labWorkflowEntityEntry.Entity;
        }

        public async ValueTask<LabWorkflow> DeleteLabWorkflowAsync(LabWorkflow workflow)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<LabWorkflow> labWorkflowEntityEntry =
               broker.LabWorkflows.Remove(workflow);

            await broker.SaveChangesAsync();

            return labWorkflowEntityEntry.Entity;
        }
    }
}
