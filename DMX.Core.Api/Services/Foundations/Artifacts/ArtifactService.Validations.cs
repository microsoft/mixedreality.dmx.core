// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.Artifacts;
using DMX.Core.Api.Models.Foundations.Artifacts.Exceptions;

namespace DMX.Core.Api.Services.Foundations.Artifacts
{
    public partial class ArtifactService
    {
        private void ValidateArtifactOnAdd(Artifact artifact)
        {
            ValidateArtifactIsNotNull(artifact);
        }

        private static void ValidateArtifactIsNotNull(Artifact artifact)
        {
            if (artifact is null)
            {
                throw new NullArtifactException();
            }
        }
    }
}
