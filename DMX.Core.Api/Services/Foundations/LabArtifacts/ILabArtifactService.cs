// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace DMX.Core.Api.Services.Foundations.LabArtifacts
{
    public interface ILabArtifactService
    {
        ValueTask AddLabArtifactAsync(string labArtifactName, Stream labArtifactContent);
    }
}
