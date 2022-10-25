// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.LabWorkflows.Exceptions
{
    public class LabWorkflowOrchestrationServiceException : Xeption
    {
        public LabWorkflowOrchestrationServiceException(Xeption innerException)
            : base(message: "Lab workflow service error occurred. Please contact support.",
                  innerException)
        { }
    }
}
