// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowEvent.Exceptions
{
    public class FailedLabWorkflowEventServiceException : Xeption
    {
        public FailedLabWorkflowEventServiceException(Exception innerException) :
            base(message: "Failed lab workflow event service exception occurred. Please contact support.",
                innerException)
        { }
    }
}
