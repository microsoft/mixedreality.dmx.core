// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace DMX.Core.Api.Tests.Acceptance.Brokers
{
    public partial class DmxCoreApiBroker
    {
        private const string HomeRelativeUrl = "api/home";

        public async ValueTask<string> GetHomeMessageAsync() => 
            await this.apiFactoryClient.GetContentStringAsync(HomeRelativeUrl);
    }
}
