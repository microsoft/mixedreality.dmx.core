// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using Microsoft.EntityFrameworkCore;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<LabDevice> LabDevices { get; set; }

        public async ValueTask<LabDevice> InsertLabDeviceAsync(LabDevice labDevice) =>
            await InsertAsync(labDevice);
        
        public async ValueTask<LabDevice> SelectLabDeviceByIdAsync(Guid labDeviceId) =>
            await FindAsync<LabDevice>(labDeviceId);
            
        public async ValueTask<LabDevice> DeleteLabDeviceAsync(LabDevice labDevice) =>
            await DeleteAsync(labDevice);
    }
}
