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

        public async ValueTask<ExternalLabsCollection> GetAvailableDevicesAsync(string reverbServiceId)
        {
            const string RelativeUrl = "api/GetAvailableDevicesAsync";

            ExternalLabsServiceInformation ReverbServiceProperties = new ExternalLabsServiceInformation
            {
                ServiceType = ReverbServiceType,
                ServiceId = reverbServiceId
            };

            return await this.PostAync<ExternalLabsServiceInformation, ExternalLabsCollection>($"{RelativeUrl}?code={this.accessKey}", ReverbServiceProperties);
        }
    }
}
