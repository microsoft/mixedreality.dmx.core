// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace DMX.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    internal partial interface ICloudBroker
    {
        ValueTask<IWebApp> CreateWebAppAsync(
            string webAppName,
            string connectionString,
            IAppServicePlan appServicePlan,
            IResourceGroup resourceGroup);
    }
}
