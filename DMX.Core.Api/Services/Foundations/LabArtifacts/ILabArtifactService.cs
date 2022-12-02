// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabArtifacts;

namespace DMX.Core.Api.Services.Foundations.LabArtifacts
{
    public interface ILabArtifactService
    {
        ValueTask AddLabArtifactAsync(LabArtifact labArtifact);
    }
}
