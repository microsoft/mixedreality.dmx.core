// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Tests.Acceptance.Models.Labs;

namespace DMX.Core.Api.Tests.Acceptance.Brokers
{
    public partial class DmxCoreApiBroker
    {
        private const string AvailableExternalLabsApiRelativeUrl = "api/labs";

        public async ValueTask<List<Lab>> GetAllLabs()
        {
            return await this.apiFactoryClient.GetContentAsync<List<Lab>>(
                relativeUrl: $"{AvailableExternalLabsApiRelativeUrl}");
        }

        public async ValueTask<Lab> PostLabAsync(Lab lab)
        {
            return await this.apiFactoryClient.PostContentAsync<Lab>(
                relativeUrl: $"{AvailableExternalLabsApiRelativeUrl}",
                content: lab);
        }

        public async ValueTask<Lab> DeleteLabByIdAsync(Guid labId)
        {
            return await this.apiFactoryClient.DeleteContentAsync<Lab>(
                relativeUrl: $"{AvailableExternalLabsApiRelativeUrl}/{labId}");
        }

        public async ValueTask<Lab> GetLabByIdAsync(Guid labId)
        {
            return await this.apiFactoryClient.GetContentAsync<Lab>(
                relativeUrl: $"{AvailableExternalLabsApiRelativeUrl}/{labId}");
        }
    }
}
