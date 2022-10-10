// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabWorkflows;
using Microsoft.EntityFrameworkCore;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void ConfigureLabWorkflow(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LabWorkflow>()
                .Ignore(labWorkflow => labWorkflow.Commands);
        }
    }
}
