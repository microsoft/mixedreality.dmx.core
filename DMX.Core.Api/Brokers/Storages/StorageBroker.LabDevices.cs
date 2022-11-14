// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using Microsoft.EntityFrameworkCore;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<LabDevice> LabDevices { get; set; }

        public async ValueTask<LabDevice> UpdateLabDeviceAsync(LabDevice labDevice) =>
            await UpdateAsync(labDevice);

        public async ValueTask<LabDevice> DeleteLabDeviceAsync(LabDevice labDevice) =>
            await DeleteAsync(labDevice);
    }
}
