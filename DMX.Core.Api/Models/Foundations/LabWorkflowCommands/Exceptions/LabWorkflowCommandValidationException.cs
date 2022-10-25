// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions
{
    public class LabWorkflowCommandValidationException : Xeption
    {
        public LabWorkflowCommandValidationException(Xeption innerException)
            : base(message: "Lab workflow command validation error(s) occurred, please try again.",
                   innerException)
        { }
    }
}
