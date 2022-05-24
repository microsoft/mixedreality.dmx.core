// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Devices.External.ExternalDevices;

namespace DMX.Core.Api.Brokers.ReverbApis
{
    public partial class ReverbApiBroker
    {
        private const string ReverbServiceType = "AzureIotHub";

        public async ValueTask<List<ExternalDevice>> GetAvailableDevicesAsync(string reverbServiceId)
        {
            string relativeUrl = "api/GetAvailableDevicesAsync";

            var ReverbServiceProperties = new
            {
                ServiceType = ReverbServiceType,
                ServiceId = reverbServiceId
            };

            return await this.PostAync<List<ExternalDevice>>(relativeUrl, ReverbServiceProperties);
        }
    }
}
