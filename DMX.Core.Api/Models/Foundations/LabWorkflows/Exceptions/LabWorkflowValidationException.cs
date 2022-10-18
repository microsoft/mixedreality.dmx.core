// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions
{
    public class LabWorkflowValidationException : Xeption
    {
        public LabWorkflowValidationException(Xeption innerException)
            : base(message: "Lab workflow validation errors occurred, please try again.",
                   innerException)
        { }
    }
}
