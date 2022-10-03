// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using Microsoft.EntityFrameworkCore;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void ConfigureLabWorkflowCommands(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LabWorkflowCommand>()
                .HasOne(command => command.LabWorkflow)
                .WithMany(workflow => workflow.Commands)
                .HasForeignKey(command => command.LabWorkflowId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
