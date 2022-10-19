// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions
{
    public class NullLabWorkflowException : Xeption
    {
        public NullLabWorkflowException()
            : base(message: "Lab workflow is null.")
        { }
    }
}
