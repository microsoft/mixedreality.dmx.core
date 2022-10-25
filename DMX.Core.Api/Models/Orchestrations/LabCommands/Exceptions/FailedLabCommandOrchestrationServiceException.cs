// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.LabCommands.Exceptions
{
    public class FailedLabCommandOrchestrationServiceException : Xeption
    {
        public FailedLabCommandOrchestrationServiceException(Exception exception)
            : base(message: "Failed lab command service error occurred, please contact support.",
                  exception)
        { }
    }
}
