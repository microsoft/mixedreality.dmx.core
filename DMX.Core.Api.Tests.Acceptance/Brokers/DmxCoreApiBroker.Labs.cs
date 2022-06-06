// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Tests.Acceptance.Models.Labs;

namespace DMX.Core.Api.Tests.Acceptance.Brokers
{
    public partial class DmxCoreApiBroker
    {
        private const string AvailableLabsApiRelativeUrl = "api/labs";

        public async ValueTask<List<Lab>> GetAllLabs()
        {
            return await this.apiFactoryClient.GetContentAsync<List<Lab>>(
                relativeUrl: $"{AvailableLabsApiRelativeUrl}");
        }
    }
}
