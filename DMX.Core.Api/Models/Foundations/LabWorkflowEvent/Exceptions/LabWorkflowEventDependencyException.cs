// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowEvent.Exceptions
{
    public class LabWorkflowEventDependencyException : Xeption
    {
        public LabWorkflowEventDependencyException(Xeption innerException)
            : base(message: "Failed lab workflow event dependency error occurred. Please contact support",
                 innerException)
        { }
    }
}
