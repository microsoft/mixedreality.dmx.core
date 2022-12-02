// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabArtifacts.Exceptions
{
    public class LabArtifactDependencyException : Xeption
    {
        public LabArtifactDependencyException(Xeption innerException)
            : base(message: "Lab artifact dependency error occured. Please contact support.",
                  innerException)
        { }
    }
}
