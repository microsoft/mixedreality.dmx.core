// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions
{
    public class InvalidLabWorkflowException : Xeption
    {
        public InvalidLabWorkflowException()
            : base(message: "Invalid lab workflow error occurred. Please fix the errors and try again.")
        { }
    }
}
