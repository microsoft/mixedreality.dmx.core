// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.Artifacts.Exceptions
{
    public class FailedArtifactDependencyException : Xeption
    {
        public FailedArtifactDependencyException(Exception innerException)
            : base(message: "Failed artifact dependency error occured. Please contact support.",
                  innerException)
        { }
    }
}
