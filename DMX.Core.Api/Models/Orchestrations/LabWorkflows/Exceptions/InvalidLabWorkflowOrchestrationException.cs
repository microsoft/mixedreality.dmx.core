// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.LabWorkflows.Exceptions
{
    public class InvalidLabWorkflowOrchestrationException : Xeption
    {
        public InvalidLabWorkflowOrchestrationException()
            : base(message: "Invalid lab workflow error occurred.Please fix the errors and try again.")
        { }
    }
}
