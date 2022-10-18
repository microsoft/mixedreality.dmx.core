// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions
{
    public class LabWorkflowCommandDependencyValidationException : Xeption
    {
        public LabWorkflowCommandDependencyValidationException(Xeption innerException)
            : base(message: "Lab workflow command dependency validation error occured. Please contact support.",
                  innerException)
        {
        }
    }
}
