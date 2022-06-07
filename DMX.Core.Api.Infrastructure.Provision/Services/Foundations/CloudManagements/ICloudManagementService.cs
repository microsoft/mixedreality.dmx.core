// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Infrastructure.Provision.Models.Storages;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;

namespace DMX.Core.Api.Infrastructure.Provision.Services.Foundations.CloudManagements
{
    public interface ICloudManagementService
    {
        ValueTask<IResourceGroup> ProvisionResourceGroupAsync(
            string projectName,
            string environment);

        ValueTask<IAppServicePlan> ProvisionAppServicePlanAsync(
            string projectName,
            string environment,
            IResourceGroup resourceGroup);

        ValueTask<ISqlServer> ProvisionSqlServerAsync(
            string projectName,
            string environment,
            IResourceGroup resourceGroup);

        ValueTask<SqlDatabase> ProvisionSqlDatabaseAsync(
            string projectName,
            string environment,
            ISqlServer sqlServer);

        ValueTask<IWebApp> ProvisionWebAppAsync(
            string projectName,
            string environment,
            string databaseConnectionString,
            IAppServicePlan appServicePlan,
            IResourceGroup resourceGroup);

        ValueTask DeprovisionResourceGroupAsync(
            string projectName,
            string environment);
    }
}
