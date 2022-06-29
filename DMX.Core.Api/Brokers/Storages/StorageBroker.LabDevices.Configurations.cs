// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.Labs;
using Microsoft.EntityFrameworkCore;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void ConfigureLabDevice(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LabDevice>()
                .HasOne(device => device.Lab)
                .WithMany(lab => lab.Devices)
                .HasForeignKey(device => device.LabId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
