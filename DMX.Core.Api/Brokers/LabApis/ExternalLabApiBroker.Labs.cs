// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.ExternalLabs;

namespace DMX.Core.Api.Brokers.LabApis
{
    public partial class ExternalLabApiBroker
    {
        const string ExternalLabDevicesRelativeUrl = "api/GetAvailableDevicesAsync";

        public async ValueTask<ExternalLabCollection> GetAvailableLabsAsync()
        {
            return await this.PostAync<ExternalLabServiceInformation, ExternalLabCollection>(
                relativeUrl: $"{ExternalLabDevicesRelativeUrl}?code={this.accessKey}",
                content: this.externalLabServiceInformation);
        }
    }
}
