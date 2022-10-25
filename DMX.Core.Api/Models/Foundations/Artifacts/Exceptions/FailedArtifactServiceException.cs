// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.Artifacts.Exceptions
{
    public class FailedArtifactServiceException : Xeption
    {
        public FailedArtifactServiceException(Exception innerException) :
            base(message: "Failed artifact service error occurred, contact support.",
                innerException)
        { }
    }
}
