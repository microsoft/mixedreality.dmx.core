// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Infrastructure.Provision.Models.Storages;
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
                    .WithRegion(Region.USWest3);

            IWithCreate sqlServerWithLogin = sqlServerWithRegion.WithExistingResourceGroup(resourceGroup)
                .WithAdministratorLogin(this.adminName)
                    .WithAdministratorPassword(this.adminAccess);

            return await sqlServerWithLogin.CreateAsync();
        }

        public async ValueTask<ISqlDatabase> CreateSqlDatabaseAsync(
            string sqlDatabaseName,
            ISqlServer sqlServer)
        {
            return await this.azure.SqlServers.Databases
                .Define(sqlDatabaseName)
                    .WithExistingSqlServer(sqlServer)
                        .CreateAsync();
        }

        public SqlDatabaseAccess GetSqlDatabaseAccess()
        {
            return new SqlDatabaseAccess
            {
                AdminName = this.adminName,
                AdminAccess = this.adminAccess,
            };
        }
    }
}
