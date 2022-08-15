// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabCommands;
using Microsoft.EntityFrameworkCore;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<LabCommand> LabCommands { get; set; }
    }
}
