// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions
{
    public class NotFoundLabWorkflowException : Xeption
    {
        public NotFoundLabWorkflowException(Guid labWorkflowId)
            : base(message: $"Could not find lab workflow with id {labWorkflowId}")
        { }
    }
}
