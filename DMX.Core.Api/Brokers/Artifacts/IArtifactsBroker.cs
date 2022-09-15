// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.Artifacts;
using System.Threading.Tasks;

namespace DMX.Core.Api.Brokers.Artifacts
{
    public interface IArtifactsBroker
    {
        ValueTask UploadArtifactAsync(Artifact artifact);
    }
}
