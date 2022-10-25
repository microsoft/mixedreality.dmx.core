// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions
{
    public class AlreadyExistsLabWorkflowException : Xeption
    {
        public AlreadyExistsLabWorkflowException(Exception innerException)
            : base(message: "Lab command with same Id already exists.",
                  innerException)
        { }
    }
}
