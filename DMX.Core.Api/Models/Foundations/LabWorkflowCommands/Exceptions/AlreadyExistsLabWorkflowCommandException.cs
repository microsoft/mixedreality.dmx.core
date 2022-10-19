// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions
{
    public class AlreadyExistsLabWorkflowCommandException : Xeption
    {
        public AlreadyExistsLabWorkflowCommandException(Exception innerException)
            : base(message: "Lab workflow command with same Id already exists.",
                  innerException)
        { }
    }
}
