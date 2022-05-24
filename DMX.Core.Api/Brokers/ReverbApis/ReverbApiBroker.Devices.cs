// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.External.ExternalLabs;

namespace DMX.Core.Api.Brokers.ReverbApis
{
    public partial class ReverbApiBroker
    {
        private const string ReverbServiceType = "AzureIotHub";

        public async ValueTask<ExternalLabCollection> GetAvailableDevicesAsync(string reverbServiceId)
        {
            const string RelativeUrl = "api/GetAvailableDevicesAsync";

            ExternalLabServiceRequest ReverbServiceProperties = new ExternalLabServiceRequest
            {
                ServiceType = ReverbServiceType,
                ServiceId = reverbServiceId
            };

            return await this.PostAync<ExternalLabServiceRequest, ExternalLabCollection>($"{RelativeUrl}?code={this.accessKey}", ReverbServiceProperties);
        }
    }
}
