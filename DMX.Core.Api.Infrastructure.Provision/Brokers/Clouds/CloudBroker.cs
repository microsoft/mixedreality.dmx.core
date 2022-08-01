// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace DMX.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker : ICloudBroker
    {
        private const string DmxEnvironment = "Production";
        private readonly string tenantId;
        private readonly string dmxCoreClientId;
        private readonly string dmxCoreInstance;
        private readonly string dmxCoreDomain;
        private readonly string dmxCoreCallbackPath;
        private readonly string dmxCoreScopesGetAllLabs;
        private readonly string dmxCoreScopesPostLab;
        private readonly string dmxCoreScopesDeleteLab;
        private readonly string provisionClientId;
        private readonly string provisionClientSecret;
        private readonly string provisionAdminName;
        private readonly string provisionAdminAccess;
        private readonly string configurationExternalLabApiUrl;
        private readonly string configurationExternalLabApiAccessKey;
        private readonly IAzure azure;

        public CloudBroker()
        {
            this.tenantId = Environment.GetEnvironmentVariable("AzureTenantId");
            this.dmxCoreClientId = Environment.GetEnvironmentVariable("AzureAdAppDmxCoreClientId");
            this.dmxCoreInstance = Environment.GetEnvironmentVariable("AzureAdAppDmxCoreInstance");
            this.dmxCoreDomain = Environment.GetEnvironmentVariable("AzureAdAppDmxCoreDomain");
            this.dmxCoreCallbackPath = Environment.GetEnvironmentVariable("AzureAdAppDmxCoreCallbackPath");
            this.dmxCoreScopesGetAllLabs = Environment.GetEnvironmentVariable("AzureAdAppDmxCoreScopesGetAllLabs");
            this.dmxCoreScopesPostLab = Environment.GetEnvironmentVariable("AzureAdAppDmxCoreScopesPostLab");
            this.dmxCoreScopesDeleteLab = Environment.GetEnvironmentVariable("AzureAdAppDmxCoreScopesDeleteLab");
            this.provisionClientId = Environment.GetEnvironmentVariable("AzureAdAppProvisionClientId");
            this.provisionClientSecret = Environment.GetEnvironmentVariable("AzureAdAppProvisionClientSecret");
            this.provisionAdminName = Environment.GetEnvironmentVariable("AzureSqlServerAdminName");
            this.provisionAdminAccess = Environment.GetEnvironmentVariable("AzureSqlServerAdminAccess");
            this.configurationExternalLabApiUrl = Environment.GetEnvironmentVariable("AzureAppServiceExternalLabApiUrl");
            this.configurationExternalLabApiAccessKey = Environment.GetEnvironmentVariable("AzureAppServiceExternalLabApiAccessKey");
            this.azure = AuthenticateAzure();
        }

        private IAzure AuthenticateAzure()
        {
            AzureCredentials credentials =
                SdkContext.AzureCredentialsFactory.FromServicePrincipal(
                    clientId: this.provisionClientId,
                    clientSecret: this.provisionClientSecret,
                    tenantId: this.tenantId,
                    environment: AzureEnvironment.AzureGlobalCloud);

            return Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                    .Authenticate(credentials)
                        .WithDefaultSubscription();
        }
    }
}
