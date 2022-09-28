// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Artifacts;

namespace DMX.Core.Api.Brokers.Artifacts
{
    public interface IArtifactsBroker
    {
        ValueTask UploadArtifactAsync(Artifact artifact);
    }
}
