// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.LabCommands.Exceptions
{
    public class LabCommandOrchestrationDependencyValidationException : Xeption
    {
        public LabCommandOrchestrationDependencyValidationException(Xeption exception)
            : base(message: "Lab command orchestration validation dependency error occured. Please fix and try again.",
                  exception)
        { }
    }
}
