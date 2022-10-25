// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.LabCommands.Exceptions
{
    public class LabCommandOrchestrationDependencyValidationException : Xeption
    {
        public LabCommandOrchestrationDependencyValidationException(Xeption innerException)
            : base(message: "Lab command validation dependency error occurred. Please fix and try again.",
                  innerException)
        { }
    }
}
