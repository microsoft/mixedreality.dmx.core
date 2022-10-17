// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions
{
    public class LabWorkflowDependencyValidationException : Xeption
    {
        public LabWorkflowDependencyValidationException(Xeption innerException)
            : base(message: "Lab workflow dependency validation error occurred, please contact support.",
                  innerException)
        { }
    }
}
