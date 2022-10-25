// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions
{
    public class LabWorkflowServiceException : Xeption
    {
        public LabWorkflowServiceException(Xeption innerException)
            : base(message: "Lab workflow service error occurred, contact support.",
                  innerException)
        {
        }
    }
}
