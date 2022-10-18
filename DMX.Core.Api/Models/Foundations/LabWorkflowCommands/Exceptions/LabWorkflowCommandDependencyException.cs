// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions
{
    public class LabWorkflowCommandDependencyException : Xeption
    {
        public LabWorkflowCommandDependencyException(Xeption innerException)
            : base(message: "Lab workflow command dependency error occured. Please contact support.",
                  innerException)
        {
        }
    }
}
