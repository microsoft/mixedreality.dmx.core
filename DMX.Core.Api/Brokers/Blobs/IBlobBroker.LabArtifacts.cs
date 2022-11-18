// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabArtifacts;

namespace DMX.Core.Api.Brokers.Blobs
{
    public partial interface IBlobBroker
    {
        ValueTask UploadLabArtifactAsync(LabArtifact labArtifact);
    }
}
