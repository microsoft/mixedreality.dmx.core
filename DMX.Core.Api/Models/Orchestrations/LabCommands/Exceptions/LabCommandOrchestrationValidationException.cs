// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.LabCommands.Exceptions
{
    public class LabCommandOrchestrationValidationException : Xeption
    {
        public LabCommandOrchestrationValidationException(Xeption innerException)
            : base(message: "Lab command validation error occurred, please try again.",
                  innerException)
        { }
    }
}
