// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions
{
    public class LabWorkflowCommandServiceException : Xeption
    {
        public LabWorkflowCommandServiceException(Xeption innerException)
            : base(message: "Lab workflow command service exception occurred. Please contact support.",
                  innerException)
        { }
    }
}
