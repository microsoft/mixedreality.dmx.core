// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions
{
    public class NullLabWorkflowCommandException : Xeption
    {
        public NullLabWorkflowCommandException()
            : base(message: "Lab workflow command is null.")
        { }
    }
}
