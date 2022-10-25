// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.LabWorkflows.Exceptions
{
    public class LabWorkflowOrchestrationValidationException : Xeption
    {
        public LabWorkflowOrchestrationValidationException(Xeption innerException)
            : base(message: "Lab workflow validation error occured. Please fix and try again.",
                  innerException)
        { }
    }
}
