// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions
{
    public class FailedLabWorkflowCommandStorageException : Xeption
    {
        public FailedLabWorkflowCommandStorageException(Exception innerException)
            : base(message: "Failed lab workflow command storage error occured. Please contact support.",
                  innerException)
        {
        }
    }
}
