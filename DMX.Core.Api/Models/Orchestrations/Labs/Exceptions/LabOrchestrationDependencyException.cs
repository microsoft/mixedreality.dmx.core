// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.Labs.Exceptions
{
    public class LabOrchestrationDependencyException : Xeption
    {
        public LabOrchestrationDependencyException(Xeption innerException)
            : base(message: "Lab dependency error occurred, please contact support.",
                   innerException)
        { }
    }
}
