// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.Artifacts.Exceptions
{
    public class NullArtifactException : Xeption
    {
        public NullArtifactException()
            : base(message: "Artifact is null.")
        {
        }
    }
}
