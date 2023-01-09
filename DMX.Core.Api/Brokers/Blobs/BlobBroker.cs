// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Azure.Storage.Blobs;
using DMX.Core.Api.Models.Configurations;
using Microsoft.Extensions.Configuration;

namespace DMX.Core.Api.Brokers.Blobs
{
    public partial class BlobBroker : IBlobBroker
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly IConfiguration configuration;
        private readonly string connectionString;
        private readonly string containerName;

        public BlobBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = GetStorageConnectionString(this.configuration);
            containerName = GetContainerName(this.configuration);
            blobServiceClient = new BlobServiceClient(connectionString);
        }

        private static string GetStorageConnectionString(IConfiguration configuration)
        {
            LabArtifactConfigurations labArtifactConfigurations =
                configuration.Get<LabArtifactConfigurations>();

            return labArtifactConfigurations.ConnectionString;
        }

        private static string GetContainerName(IConfiguration configuration)
        {
            LabArtifactConfigurations labArtifactConfigurations =
                configuration.Get<LabArtifactConfigurations>();

            return labArtifactConfigurations.ContainerName;
        }
    }
}
