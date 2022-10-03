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

        public async ValueTask<LabWorkflow> InsertLabWorkflowAsync(LabWorkflow workflow) =>
            await InsertAsync(workflow);

        public async ValueTask<LabWorkflow> SelectLabWorkflowByIdAsync(Guid labWorkflowId) =>
            await SelectAsync<LabWorkflow>(labWorkflowId);

        public async ValueTask<LabWorkflow> UpdateLabWorkflowAsync(LabWorkflow workflow) =>
            await UpdateAsync(workflow);

        public async ValueTask<LabWorkflow> DeleteLabWorkflowAsync(LabWorkflow workflow) =>
            await DeleteAsync(workflow);
    }
}
