// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabArtifacts.Exceptions
{
    public class FailedLabArtifactDependencyException : Xeption
    {
        public FailedLabArtifactDependencyException(Exception innerException)
            : base(message: "Failed lab artifact dependency error occured. Please contact support.",
                  innerException)
        { }
    }
}
