// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabArtifacts;

namespace DMX.Core.Api.Tests.Acceptance.Brokers
{
    public partial class DmxCoreApiBroker
    {
        private const string LabArtifactsApiRelativeUrl = "api/labartifacts";
        private const string LabArtifactsMediaType = "application/octet-stream";

        public async ValueTask PostLabArtifactAsync(LabArtifact labArtifact) =>
            await this.apiFactoryClient.PostContentWithNoResponseAsync(
                relativeUrl: $"{LabArtifactsApiRelativeUrl}/{labArtifact.Name}",
                content: labArtifact.Content,
                mediaType: LabArtifactsMediaType);
    }
}