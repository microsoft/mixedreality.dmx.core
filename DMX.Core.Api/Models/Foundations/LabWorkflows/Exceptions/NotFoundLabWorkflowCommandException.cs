// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions
{
    public class NotFoundLabWorkflowCommandException : Xeption
    {
        public NotFoundLabWorkflowCommandException(Guid id)
            :base(message: $"Lab workflow command with Id {id} not found")
        { }
    }
}
