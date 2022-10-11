// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using Microsoft.EntityFrameworkCore;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<LabWorkflow> LabWorkflows { get; set; }

        public async ValueTask<LabWorkflow> InsertLabWorkflowAsync(LabWorkflow labWorkflow) =>
            await InsertAsync(labWorkflow);

        public async ValueTask<LabWorkflow> SelectLabWorkflowByIdAsync(Guid Id) =>
            await SelectAsync<LabWorkflow>(Id);

        public async ValueTask<LabWorkflow> UpdateLabWorkflowAsync(LabWorkflow labWorkflow) =>
            await UpdateAsync(labWorkflow);

        public async ValueTask<LabWorkflow> DeleteLabWorkflowAsync(LabWorkflow labWorkflow) =>
            await DeleteAsync(labWorkflow);
    }
}