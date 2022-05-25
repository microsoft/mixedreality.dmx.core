// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.AppService.Fluent.Models;
using Microsoft.Azure.Management.AppService.Fluent.WebApp.Definition;
using Microsoft.Azure.Management.AppService.Fluent.WebAppBase.Definition;
using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace DMX.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<IWebApp> CreateWebAppAsync(
            string webAppName,
            string connectionString,
            IAppServicePlan appServicePlan,
            IResourceGroup resourceGroup)
        {
            IExistingWindowsPlanWithGroup webAppWithExistingWindowsPlan = this.azure.AppServices.WebApps
                .Define(webAppName)
                    .WithExistingWindowsPlan(appServicePlan);

            IWithCreate<IWebApp> webAppWithConnectionString = webAppWithExistingWindowsPlan.WithExistingResourceGroup(resourceGroup)
                .WithNetFrameworkVersion(NetFrameworkVersion.Parse("v7.0"))
                    .WithConnectionString(
                        name: "DefaultConnection",
                        value: connectionString,
                        type: ConnectionStringType.SQLAzure);

            return await webAppWithConnectionString.CreateAsync();
        }
    }
}
