// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions
{
    public class NullLabWorkflowCommandException : Xeption
    {
        public NullLabWorkflowCommandException()
            : base(message: "Lab workflow command is null.")
        {
        }
    }
}
