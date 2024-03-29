﻿// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.LabWorkflows.Exceptions
{
    public class LabWorkflowOrchestrationDependencyException : Xeption
    {
        public LabWorkflowOrchestrationDependencyException(Xeption innerException)
            : base(message: "Lab workflow dependency error occurred. Please contact support.",
                  innerException)
        { }
    }
}
