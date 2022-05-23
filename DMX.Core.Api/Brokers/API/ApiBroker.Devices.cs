// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Devices;

namespace DMX.Core.Api.Brokers.API
{
    public partial class ApiBroker
    {
        private const string RelativeUrl = "api/GetAvailableDevicesAsync";

        public async ValueTask<List<Device>> GetAvailableDevices() => await this.GetAsync<List<Device>>(RelativeUrl);
    }
}
