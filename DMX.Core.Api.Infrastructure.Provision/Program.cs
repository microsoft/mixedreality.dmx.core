using System;
using System.Threading.Tasks;
using DMX.Core.Api.Infrastructure.Provision.Models.Storages;
using DMX.Core.Api.Infrastructure.Provision.Services.Foundations.CloudManagements;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;

public class Program
{
    public async static Task Main(string[] args)
    {
        var cloudManagementService = new CloudManagementService();
        var projectName = "DMX-CORE-API";
        var environment = "DEV";

        IResourceGroup resourceGroup = await cloudManagementService.ProvisionResourceGroupAsync(projectName, environment);
        IAppServicePlan appServicePlan = await cloudManagementService.ProvisionAppServicePlanAsync(projectName, environment, resourceGroup);
        ISqlServer sqlServer = await cloudManagementService.ProvisionSqlServerAsync(projectName, environment, resourceGroup);

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