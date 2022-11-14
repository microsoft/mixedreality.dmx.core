// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<LabDevice> InsertLabDeviceAsync(LabDevice labDevice);
        ValueTask<LabDevice> DeleteLabDeviceAsync(LabDevice labDevice);
    }
}
