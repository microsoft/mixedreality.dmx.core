// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.Artifacts.Exceptions
{
    public class ArtifactServiceException : Xeption
    {
        public ArtifactServiceException(Xeption innerException)
            : base(message: "Artifact service error occurred, contact support.",
                  innerException)
        { }
    }
}
