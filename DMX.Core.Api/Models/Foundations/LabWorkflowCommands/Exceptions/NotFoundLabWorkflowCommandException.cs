// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions
{
    public class NotFoundLabWorkflowCommandException : Xeption
    {
        public NotFoundLabWorkflowCommandException(Guid labWorkflowId)
            : base(message: $"Could not find lab workflow command with id {labWorkflowId}")
        { }
    }
}
