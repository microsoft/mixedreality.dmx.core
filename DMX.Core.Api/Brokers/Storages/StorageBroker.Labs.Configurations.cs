// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.Labs;
using Microsoft.EntityFrameworkCore;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void ConfigureLab(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lab>()
                .Ignore(lab => lab.Devices);
        }
    }
}
