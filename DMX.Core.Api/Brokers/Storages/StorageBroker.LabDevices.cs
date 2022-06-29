// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.Labs;
using Microsoft.EntityFrameworkCore;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<LabDevice> LabDevices { get; set; }
    }
}
