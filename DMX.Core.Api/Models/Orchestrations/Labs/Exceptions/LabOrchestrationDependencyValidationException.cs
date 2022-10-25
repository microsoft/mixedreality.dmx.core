// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.Labs.Exceptions
{
    public class LabOrchestrationDependencyValidationException : Xeption
    {
        public LabOrchestrationDependencyValidationException(Xeption innerException)
            : base(message: "Lab validation dependency error occurred. Please fix and try again.",
                  innerException)
        { }
    }
}
