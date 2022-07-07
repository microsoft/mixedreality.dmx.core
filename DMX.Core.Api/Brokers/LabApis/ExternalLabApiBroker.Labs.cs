// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.ExternalLabs;

namespace DMX.Core.Api.Brokers.LabApis
{
    public partial class ExternalLabApiBroker
    {
        public async ValueTask<ExternalLabCollection> GetAvailableLabsAsync(
            ExternalLabServiceInformation externalLabServiceInformation)
        {
            const string RelativeUrl = "api/GetAvailableDevicesAsync";

            externalLabServiceInformation.ServiceType =
                this.externalLabServiceInformation.ServiceType;

            externalLabServiceInformation.ServiceId =
                this.externalLabServiceInformation.ServiceId;

            return await this.PostAync<ExternalLabServiceInformation, ExternalLabCollection>(
                relativeUrl: $"{RelativeUrl}?code={this.accessKey}",
                content: externalLabServiceInformation);
        }
    }
}
