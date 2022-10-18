// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions
{
    public class FailedLabWorkflowCommandServiceException : Xeption
    {
        public FailedLabWorkflowCommandServiceException(Exception innerException)
            : base(message: "Failed lab workflow command service error occurred, contact support.",
                  innerException)
        {
        }
    }
}
