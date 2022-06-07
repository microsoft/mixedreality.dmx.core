// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Infrastructure.Provision.Brokers.Configurations;
using DMX.Core.Api.Infrastructure.Provision.Models.Configurations;
using DMX.Core.Api.Infrastructure.Provision.Models.Storages;
using DMX.Core.Api.Infrastructure.Provision.Services.Foundations.CloudManagements;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;

namespace DMX.Core.Api.Infrastructure.Provision.Services.Processings.CloudManagements
{
    public class CloudManagementProcessingService : ICloudManagementProcessingService
    {
        private readonly IConfigurationBroker configurationBroker;
        private readonly ICloudManagementService cloudManagementService;

        public CloudManagementProcessingService()
        {
            this.configurationBroker = new ConfigurationBroker();
            this.cloudManagementService = new CloudManagementService();
        }

        public async ValueTask ProcessAsync()
        {
            CloudManagementConfiguration configuration = this.configurationBroker.GetConfiguration();
            await ProvisionResourcesAsync(configuration);
            await DeprovisionResourcesAsync(configuration);
        }
        private async Task ProvisionResourcesAsync(CloudManagementConfiguration configuration)
        {
            string projectName = configuration.ProjectName;

            List<string> provisionEnvironments = RetrieveEnvironments(cloudAction: configuration.Up);

            foreach (string environment in provisionEnvironments)
            {
                IResourceGroup resourceGroup =
                await cloudManagementService.ProvisionResourceGroupAsync(projectName, environment);

                IAppServicePlan appServicePlan =
                    await cloudManagementService.ProvisionAppServicePlanAsync(projectName, environment, resourceGroup);

                ISqlServer sqlServer = await
                    cloudManagementService.ProvisionSqlServerAsync(projectName, environment, resourceGroup);

                SqlDatabase sqlDatabase = await cloudManagementService.ProvisionSqlDatabaseAsync(
                    projectName,
                    environment,
                    sqlServer);

                var webApp = await cloudManagementService.ProvisionWebAppAsync(
                    projectName,
                    environment,
                    sqlDatabase.ConnectionString,
                    appServicePlan,
                    resourceGroup);
            }
        }

        private async Task DeprovisionResourcesAsync(CloudManagementConfiguration configuration)
        {
            string projectName = configuration.ProjectName;

            List<string> provisionEnvironments = RetrieveEnvironments(cloudAction: configuration.Down);

            foreach (string environment in provisionEnvironments)
            {
                await this.cloudManagementService.DeprovisionResourceGroupAsync(projectName, environment);
            }
        }

        private static List<string> RetrieveEnvironments(CloudAction cloudAction) =>
            cloudAction?.Environments ?? new List<string>();
    }
}
