// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Artifacts;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Models.Foundations.Artifacts;

namespace DMX.Core.Api.Services.Foundations.Artifacts
{
    public partial class ArtifactService : IArtifactService
    {
        private readonly IArtifactsBroker artifactsBroker;
        private readonly ILoggingBroker loggingBroker;

        public ArtifactService(
            IArtifactsBroker artifactsBroker,
            ILoggingBroker loggingBroker)
        {
            this.artifactsBroker = artifactsBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask AddArtifactAsync(Artifact artifact) =>
            this.artifactsBroker.UploadArtifactAsync(artifact);
    }
}
