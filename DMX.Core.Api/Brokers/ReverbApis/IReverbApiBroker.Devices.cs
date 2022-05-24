// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Devices.External.ExternalDevices;

namespace DMX.Core.Api.Brokers.ReverbApis
{
    public partial interface IReverbApiBroker
    {
        ValueTask<List<ExternalDevice>> GetAvailableDevicesAsync(string reverbServiceId);
    }
}
