// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions
{
    public class FailedLabWorkflowServiceException : Xeption
    {
        public FailedLabWorkflowServiceException(Exception innerException)
            : base(message: "Failed lab workflow service error occurred, contact support.",
                  innerException)
        {
        }
    }
}
