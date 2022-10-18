// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions
{
    public class FailedLabWorkflowStorageException : Xeption
    {
        public FailedLabWorkflowStorageException(Exception innerException)
            : base(message: "Failed lab workflow storage error occurred, please contact support.",
                  innerException)
        { }
    }
}
