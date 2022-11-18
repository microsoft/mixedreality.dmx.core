// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Azure.Storage.Blobs;
using DMX.Core.Api.Models.Foundations.LabArtifacts;

namespace DMX.Core.Api.Brokers.Blobs
{
    public partial class BlobBroker : IBlobBroker
    {
        public async ValueTask UploadLabArtifactAsync(LabArtifact labArtifact)
        {
            BlobClient blobClient = blobContainerClient.GetBlobClient(labArtifact.Name);
            await blobClient.UploadAsync(labArtifact.Content);
        }
    }
}
