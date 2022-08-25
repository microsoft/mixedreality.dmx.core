// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.ExternalLabs;

namespace DMX.Core.Api.Brokers.LabApis
{
    public partial class ExternalLabApiBroker
    {
        const string AvailableExternalLabDevicesRelativeUrl = "api/GetAvailableDevicesAsync";
        const string AllExternalLabDevicesRelativeUrl = "api/GetAllDevicesAsync";

        public async ValueTask<ExternalLabCollection> GetAvailableLabsAsync()
        {
            return await this.PostAync<ExternalLabServiceInformation, ExternalLabCollection>(
                relativeUrl: $"{AvailableExternalLabDevicesRelativeUrl}?code={this.availableDevicesAccessKey}",
                content: this.externalLabServiceInformation);
        }

        public async ValueTask<ExternalLabCollection> GetAllLabsAsync()
        {
            return await this.PostAync<ExternalLabServiceInformation, ExternalLabCollection>(
                relativeUrl: $"{AllExternalLabDevicesRelativeUrl}?code={this.allDevicesAccessKey}",
                content: this.externalLabServiceInformation);
        }
    }
}
