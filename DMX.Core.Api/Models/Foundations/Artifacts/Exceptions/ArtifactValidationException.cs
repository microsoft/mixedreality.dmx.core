// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.Artifacts.Exceptions
{
    public class ArtifactValidationException : Xeption
    {
        public ArtifactValidationException(Xeption innerException)
            : base(message: "Artifact validation error occured. Please fix and try again.",
                  innerException)
        {
        }
    }
}
