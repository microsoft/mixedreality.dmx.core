// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Sql.Fluent;
using Microsoft.Azure.Management.Sql.Fluent.SqlServer.Definition;

namespace DMX.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<ISqlServer> CreateSqlServerAsync(
            string sqlServerName,
            IResourceGroup resourceGroup)
        {
            IWithGroup sqlServerWithRegion = this.azure.SqlServers
                .Define(sqlServerName)
                    .WithRegion(Region.USWest);

            IWithCreate sqlServerWithLogin = sqlServerWithRegion.WithExistingResourceGroup(resourceGroup)
                .WithAdministratorLogin(this.adminName)
                    .WithAdministratorPassword(this.adminAccess);

            return await sqlServerWithLogin.CreateAsync();
        }
    }
}
