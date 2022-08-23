// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.LabCommands.Exceptions
{
    public class LabCommandOrchestrationServiceException : Xeption
    {
        public LabCommandOrchestrationServiceException(Xeption innerException)
            : base(message: "Lab command orchestration service error occurred. Please contact support",
                  innerException)
        { }
    }
}
