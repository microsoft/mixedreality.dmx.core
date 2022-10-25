// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.LabWorkflows.Exceptions
{
    public class LabWorkflowOrchestrationDependencyValidationException : Xeption
    {
        public LabWorkflowOrchestrationDependencyValidationException(Xeption innerException)
            : base(message: "Lab workflow dependency validation error occured. Please fix and try again.",
                  innerException)
        { }
    }
}
