// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.Labs.Exceptions
{
    public class LabOrchestrationDependencyValidationException : Xeption
    {
        public LabOrchestrationDependencyValidationException(Xeption exception)
            : base(message: "Lab validation dependency error occured. Please fix and try again.",
                  exception)
        { }
    }
}
