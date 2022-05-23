// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Devices;

namespace DMX.Core.Api.Brokers.API
{
    public partial interface IApiBroker
    {
        ValueTask<List<Device>> GetAvailableDevicesAsync();
    }
}
