// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.Artifacts.Exceptions
{
    public class ArtifactDependencyException : Xeption
    {
        public ArtifactDependencyException(Xeption innerException)
            : base(message: "Artifact dependency error occured. Please contact support.",
                  innerException)
        { }
    }
}
