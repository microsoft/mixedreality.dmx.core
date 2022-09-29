// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.IO;

namespace DMX.Core.Api.Models.Foundations.Artifacts
{
    public class Artifact
    {
        public string Name { get; set; }
        public Stream Content { get; set; }
    }
}
