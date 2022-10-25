// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.Labs.Exceptions
{
    public class FailedLabOrchestrationServiceException : Xeption
    {
        public FailedLabOrchestrationServiceException(Exception innerException)
            : base(message: "Failed lab service error occured. Please contact support.",
                  innerException)
        { }
    }
}
