// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Infrastructure.Provision.Brokers.Clouds;
using DMX.Core.Api.Infrastructure.Provision.Brokers.Loggings;
using DMX.Core.Api.Infrastructure.Provision.Models.Storages;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;

namespace DMX.Core.Api.Infrastructure.Provision.Services.Foundations.CloudManagements
{
    public class CloudManagementService : ICloudManagementService
    {
        private readonly ICloudBroker cloudBroker;
        private readonly ILoggingBroker loggingBroker;

        public CloudManagementService()
        {
            this.cloudBroker = new CloudBroker();
            this.loggingBroker = new LoggingBroker();
        }

        public async ValueTask<IResourceGroup> ProvisionResourceGroupAsync(
            string projectName,
            string environment)
        {
            string resourceGroupName = $"{projectName}-RESOURCES-{environment}".ToUpper();

            this.loggingBroker.LogActivity(
                message: $"Starting provisioning {resourceGroupName} ...");

            IResourceGroup resourceGroup =
                await this.cloudBroker.CreateResourceGroupAsync(
                    resourceGroupName);

            this.loggingBroker.LogActivity(
                message: $"Provisioning {resourceGroupName} Completed.");

            return resourceGroup;
        }

        public async ValueTask<IAppServicePlan> ProvisionAppServicePlanAsync(
            string projectName,
            string environment,
            IResourceGroup resourceGroup)
        {
            string appServicePlanName = $"{projectName}-PLAN-{environment}".ToUpper();
            this.loggingBroker.LogActivity(message: $"Provisioning {appServicePlanName} ...");

            IAppServicePlan appServicePlan = await this.cloudBroker.CreatePlanAsync(
                appServicePlanName, resourceGroup);

            this.loggingBroker.LogActivity(message: $"Provisioning {appServicePlanName} complete.");

            return appServicePlan;
        }

        public async ValueTask<ISqlServer> ProvisionSqlServerAsync(
            string projectName,
            string environment,
            IResourceGroup resourceGroup)
        {
            string sqlServerName = $"{projectName}-dbserver-{environment}".ToLower();
            this.loggingBroker.LogActivity(message: $"Provisioning {sqlServerName} ...");

            ISqlServer sqlServer = await this.cloudBroker.CreateSqlServerAsync(
                sqlServerName,
                resourceGroup);

            this.loggingBroker.LogActivity(message: $"Provisioning {sqlServerName} complete.");

            return sqlServer;
        }

        public async ValueTask<SqlDatabase> ProvisionSqlDatabaseAsync(
            string projectName,
            string environment,
            ISqlServer sqlServer)
        {
            string databaseName = $"{projectName}-db-{environment}".ToLower();
            this.loggingBroker.LogActivity(message: $"Provisioning {databaseName} ...");

            ISqlDatabase sqlDatabase = await this.cloudBroker.CreateSqlDatabaseAsync(
                databaseName,
                sqlServer);

            this.loggingBroker.LogActivity(message: $"Provisioning {databaseName} complete.");

            return new SqlDatabase
            {
                Database = sqlDatabase,
                ConnectionString = GenerateConnectionString(sqlDatabase)
            };

        }

        public async ValueTask<IWebApp> ProvisionWebAppAsync(
            string projectName,
            string environment,
            string databaseConnectionString,
            IAppServicePlan appServicePlan,
            IResourceGroup resourceGroup)
        {
            string webAppName = $"{projectName}-{environment}".ToLower();
            this.loggingBroker.LogActivity(message: $"Provisioning {webAppName} ...");

            IWebApp webApp = await this.cloudBroker.CreateWebAppAsync(
                webAppName,
                databaseConnectionString,
                appServicePlan,
                resourceGroup);

            this.loggingBroker.LogActivity(message: $"Provisioning {webAppName} complete.");

            return webApp;
        }

        public async ValueTask DeprovisionResourceGroupAsync(string projectName, string environment)
        {
            string resourceGroupName = $"{projectName}-RESOURCES-{environment}".ToUpper();
            this.loggingBroker.LogActivity(message: $"Checking for {resourceGroupName} ...");
            bool isResourceGroupExist = await this.cloudBroker.CheckResourceGroupExistsAsync(resourceGroupName);

            if(isResourceGroupExist)
            {
                this.loggingBroker.LogActivity(message: $"Deprovisioning {resourceGroupName} ...");
                await this.cloudBroker.DeleteResourceGroupAsync(resourceGroupName);
                this.loggingBroker.LogActivity(message: $"Deprovisioning {resourceGroupName} completed.");
            }
            else
            {
                this.loggingBroker.LogActivity(message: $"Could not find {resourceGroupName}.");
            }

        }

        private string GenerateConnectionString(ISqlDatabase sqlDatabase)
        {
            SqlDatabaseAccess access = this.cloudBroker.GetSqlDatabaseAccess();

            return $"Server=tcp:{sqlDatabase.SqlServerName}.database.windows.net,1433;" +
                $"Initial Catalog={sqlDatabase.Name};" +
                $"User ID={access.AdminName};" +
                $"Password={access.AdminAccess};";
        }
    }
}
