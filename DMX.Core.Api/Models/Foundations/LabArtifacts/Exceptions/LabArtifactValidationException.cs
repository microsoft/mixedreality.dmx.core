// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabArtifacts.Exceptions
{
    public class LabArtifactValidationException : Xeption
    {
        public LabArtifactValidationException(Xeption innerException)
            : base(message: "Lab artifact validation error occured. Please fix and try again.",
                  innerException)
        {
        }
    }
}
