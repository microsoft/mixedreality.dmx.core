// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Azure.Storage.Blobs;
using DMX.Core.Api.Models.Configurations;
using DMX.Core.Api.Models.Foundations.Artifacts;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace DMX.Core.Api.Brokers.Artifacts
{
    public class ArtifactsBroker : IArtifactsBroker
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly BlobContainerClient blobContainerClient;
        private readonly IConfiguration configuration;
        private readonly string connectionString;
        private readonly string containerName;

        public ArtifactsBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionString = GetStorageConnectionString(this.configuration);
            this.containerName = GetContainerName(this.configuration);
            this.blobServiceClient = new BlobServiceClient(connectionString);
            this.blobContainerClient = this.blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async ValueTask UploadArtifactAsync(Artifact artifact)
        {
            BlobClient blobClient = this.blobContainerClient.GetBlobClient(artifact.Name);
            await blobClient.UploadAsync(artifact.Content, false);
        }

        private string GetStorageConnectionString(IConfiguration configuration)
        {
            ArtifactConfigurations artifactConfigurations =
                configuration.Get<ArtifactConfigurations>();

            return artifactConfigurations.ConnectionString;
        }

        private string GetContainerName(IConfiguration configuration)
        {
            ArtifactConfigurations artifactConfigurations =
                configuration.Get<ArtifactConfigurations>();

            return artifactConfigurations.ContainerName;
        }
    }
}
