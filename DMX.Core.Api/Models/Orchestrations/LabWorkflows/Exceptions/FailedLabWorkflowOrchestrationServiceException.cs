﻿// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.LabWorkflows.Exceptions
{
    public class FailedLabWorkflowOrchestrationServiceException : Xeption
    {
        public FailedLabWorkflowOrchestrationServiceException(Exception innerException)
            : base(message: "Failed lab workflow service error occurred. Please contact support.",
                  innerException)
        { }
    }
}
