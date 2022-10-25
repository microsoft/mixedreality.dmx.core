// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using Microsoft.EntityFrameworkCore;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void ConfigureLabWorkflowCommand(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LabWorkflowCommand>()
                 .HasOne(labWorkflowCommand => labWorkflowCommand.Workflow)
                 .WithMany(labWorkflow => labWorkflow.Commands)
                 .HasForeignKey(labWorkflowCommand => labWorkflowCommand.WorkflowId)
                 .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
