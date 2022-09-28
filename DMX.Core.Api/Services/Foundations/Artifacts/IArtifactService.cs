// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Artifacts;

namespace DMX.Core.Api.Services.Foundations.Artifacts
{
    public interface IArtifactService
    {
        ValueTask<Artifact> AddArtifactAsync(Artifact artifact);
    }
}
