// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions
{
    public class LockedLabWorkflowCommandException : Xeption
    {
        public LockedLabWorkflowCommandException(Exception innerException)
            : base(message: "Locked lab workflow command error occured. Please try again later.",
                  innerException)
        {
        }
    }
}
