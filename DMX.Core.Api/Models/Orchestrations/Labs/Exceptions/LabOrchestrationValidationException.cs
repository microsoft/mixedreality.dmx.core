// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.Labs.Exceptions
{
    public class LabOrchestrationValidationException : Xeption
    {
        public LabOrchestrationValidationException(Xeption innerException)
            : base(message: "Lab orchestration validation error occurred, please try again.",
                  innerException)
        { }
    }
}
