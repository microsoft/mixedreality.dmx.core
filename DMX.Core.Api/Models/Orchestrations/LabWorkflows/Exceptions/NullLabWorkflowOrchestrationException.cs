// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.LabWorkflows.Exceptions
{
    public class NullLabWorkflowOrchestrationException : Xeption
    {
        public NullLabWorkflowOrchestrationException()
            : base(message: "Lab workflow is null. Please fix and try again.")
        { }
    }
}
