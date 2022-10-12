// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions
{
    public class LabWorkflowCommandValidationException : Xeption
    {
        public LabWorkflowCommandValidationException(Xeption innerException)
            : base(message: "Lab workflow command validation error occured. Please fix and try again.",
                  innerException)
        {
        }
    }
}
