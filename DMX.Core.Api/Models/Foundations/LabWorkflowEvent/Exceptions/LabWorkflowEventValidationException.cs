// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowEvent.Exceptions
{
    public class LabWorkflowEventValidationException : Xeption
    {
        public LabWorkflowEventValidationException(Xeption innerException)
            : base(message: "Lab workflow event validation error occurred. Please fix and try again.",
                  innerException)
        { }
    }
}
