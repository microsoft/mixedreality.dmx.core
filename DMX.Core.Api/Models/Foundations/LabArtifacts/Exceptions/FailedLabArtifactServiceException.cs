// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabArtifacts.Exceptions
{
    public class FailedLabArtifactServiceException : Xeption
    {
        public FailedLabArtifactServiceException(Exception innerException) :
            base(message: "Failed lab artifact service error occurred, contact support.",
                innerException)
        { }
    }
}
