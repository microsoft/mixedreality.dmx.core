// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowEvent.Exceptions
{
    public class FailedLabWorkflowEventDependencyException : Xeption
    {
        public FailedLabWorkflowEventDependencyException(Exception innerException)
            : base(message: "Failed lab workflow event dependency error occurred. Please contact support",
                 innerException)
        { }
    }
}
