// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.Labs.Exceptions
{
    public class LabOrchestrationServiceException : Xeption
    {
        public LabOrchestrationServiceException(Xeption exception)
            : base(message: "Lab orchestration service error occured. Please contact support.", exception)
        { }
    }
}
