// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowEvent.Exceptions
{
    public class InvalidLabWorkflowEventException : Xeption
    {
        public InvalidLabWorkflowEventException(Exception exception)
            : base(message: "Invalid lab workflow event error occurred. Please try again",
                 exception)
        { }
    }
}
