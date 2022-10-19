// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowEvent.Exceptions
{
    public class LabWorkflowEventDependencyValidationException :Xeption
    {
        public LabWorkflowEventDependencyValidationException(Xeption exception)
            :base(message:" Lab Workflow event dependency validation error occurred. Please contact support",
                 exception)
        { }
    }
}
