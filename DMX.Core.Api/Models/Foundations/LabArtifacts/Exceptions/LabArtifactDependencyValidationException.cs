// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabArtifacts.Exceptions
{
    public class LabArtifactDependencyValidationException : Xeption
    {
        public LabArtifactDependencyValidationException(Xeption innerException)
            : base(message: "Lab artifact dependency validation error occurred, please contact support.",
                  innerException)
        { }
    }
}
