// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.LabWorkflows
{
    public class LabWorkflowOrchestrationValidationException : Xeption
    {
        public LabWorkflowOrchestrationValidationException(Xeption innerException)
            : base(message: "Lab workflow orchestration validation error occured. Please fix and try again.",
                  innerException)
        { }
    }
}
