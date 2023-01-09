// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabArtifacts.Exceptions
{
    public class LabArtifactServiceException : Xeption
    {
        public LabArtifactServiceException(Xeption innerException)
            : base(message: "Lab artifact service error occurred, contact support.",
                  innerException)
        { }
    }
}
