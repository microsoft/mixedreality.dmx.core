// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

namespace DMX.Core.Api.Infrastructure.Build.Services.ScriptGenerations
{
    public class ScriptGenerationService
    {
        private readonly ADotNetClient adotNetClient;

        public ScriptGenerationService() =>
            this.adotNetClient = new ADotNetClient();

        public void GenerateBuildScript()
        {
            var githubPipeline = new GithubPipeline
            {
                Name = "DMX Core Build",

                OnEvents = new Events
                {
                    Push = new PushEvent
                    {
                        Branches = new string[] { "main" }
                    },

                    PullRequest = new PullRequestEvent
                    {
                        Branches = new string[] { "main" }
                    }
                },

                Jobs = new Jobs
                {
                    Build = new BuildJob
                    {
                        RunsOn = BuildMachines.Windows2022,

                        Steps = new List<GithubTask>
                        {
                            new CheckoutTaskV2
                            {
                                Name = "Checking out code"
                            },

                            new SetupDotNetTaskV1
                            {
                                Name = "Installing .NET",

                                TargetDotNetVersion = new TargetDotNetVersion
                                {
                                    DotNetVersion = "7.0.100-preview.4.22252.9",
                                    IncludePrerelease = true
                                }
                            },

                            new RestoreTask
                            {
                                Name = "Restoring Packages"
                            },

                            new DotNetBuildTask
                            {
                                Name = "Building Project(s)"
                            },

                            new TestTask
                            {
                                Name = "Running Tests"
                            }
                        }
                    },
                }
            };

            this.adotNetClient.SerializeAndWriteToFile(
                githubPipeline,
                path: "../../../../.github/workflows/dotnet.yml");
        }

        public void GenerateProvisionScript()
        {
            var githubPipeline = new GithubPipeline
            {
                Name = "Provision DMX Core",

                OnEvents = new Events
                {
                    Push = new PushEvent
                    {
                        Branches = new string[] { "main" }
                    },

                    PullRequest = new PullRequestEvent
                    {
                        Branches = new string[] { "main" }
                    }
                },

                Jobs = new Jobs
                {
                    Build = new BuildJob
                    {
                        RunsOn = BuildMachines.WindowsLatest,

                        EnvironmentVariables = new Dictionary<string, string>
                        {
                            { "AzureSubscriptionId", "${{ secrets.AZURE_SUBSCRIPTIONID }}"},
                            { "AzureTenantId", "${{ secrets.AZURE_TENANTID }}" },
                            { "AzureAdAppProvisionClientId", "${{ secrets.AZURE_ADAPP_PROVISION_CLIENTID }}" },
                            { "AzureAdAppProvisionClientSecret", "${{ secrets.AZURE_ADAPP_PROVISION_CLIENTSECRET }}" },
                            { "AzureAdAppDmxCoreClientId", "${{ secrets.AZURE_ADAPP_DMXCORE_CLIENTID }}" },
                            { "AzureAdAppDmxCoreInstance", "${{ secrets.AZURE_ADAPP_DMXCORE_INSTANCE }}" },
                            { "AzureAdAppDmxCoreDomain", "${{ secrets.AZURE_ADAPP_DMXCORE_DOMAIN }}" },
                            { "AzureAdAppDmxCoreCallbackPath", "${{ secrets.AZURE_ADAPP_DMXCORE_CALLBACKPATH }}" },
                            { "AzureAdAppDmxCoreScopesGetAllLabs", "${{ secrets.AZURE_ADAPP_DMXCORE_SCOPES_GETALLLABS }}" },
                            { "AzureAdAppDmxCoreScopesPostLab", "${{ secrets.AZURE_ADAPP_DMXCORE_SCOPES_POSTLAB }}" },
                            { "AzureAdAppDmxCoreScopesDeleteLab", "${{ secrets.AZURE_ADAPP_DMXCORE_SCOPES_DELETELAB }}" },
                            { "AzureSqlServerAdminName", "${{ secrets.AZURE_SQLSERVER_ADMINNAME }}" },
                            { "AzureSqlServerAdminAccess", "${{ secrets.AZURE_SQLSERVER_ADMINACCESS }}" },
                            { "AzureAppServiceExternalLabApiUrl", "${{ secrets.AZURE_APPSERVICE_EXTERNALLABAPI_URL }}" },
                            { "AzureAppServiceExternalLabApiGetAllDevicesAccessKey", "${{ secrets.AZURE_APPSERVICE_EXTERNALLABAPI_GETALLDEVICES_ACCESSKEY }}" },
                            { "AzureAppServiceExternalLabApiGetAvailableDevicesAccessKey", "${{ secrets.AZURE_APPSERVICE_EXTERNALLABAPI_GETAVAILABLEDEVICES_ACCESSKEY }}" }
                        },

                        Steps = new List<GithubTask>
                        {
                            new CheckoutTaskV2
                            {
                                Name = "Check Out"
                            },

                            new SetupDotNetTaskV1
                            {
                                Name = "Setup Dot Net Version",

                                TargetDotNetVersion = new TargetDotNetVersion
                                {
                                    DotNetVersion = "7.0.100-preview.1.22110.4",
                                    IncludePrerelease = true
                                }
                            },

                            new RestoreTask
                            {
                                Name = "Restore"
                            },

                            new DotNetBuildTask
                            {
                                Name = "Build"
                            },

                            new RunTask
                            {
                                Name = "Provision",
                                Run = "dotnet run --project .\\DMX.Core.Api.Infrastructure.Provision\\DMX.Core.Api.Infrastructure.Provision.csproj"
                            }
                        }
                    }
                }
            };

            this.adotNetClient.SerializeAndWriteToFile(
                githubPipeline,
                path: "../../../../.github/workflows/provision.yml");
        }
    }
}
