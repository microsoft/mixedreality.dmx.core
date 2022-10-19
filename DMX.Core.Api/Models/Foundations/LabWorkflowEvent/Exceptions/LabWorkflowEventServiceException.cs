// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowEvent.Exceptions
{
    public class LabWorkflowEventServiceException : Xeption
    {
        public LabWorkflowEventServiceException(Xeption innerException)
            : base(message: "Lab workflow event service error occured. Please contact support.",
                  innerException)
        { }
    }
}
