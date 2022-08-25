// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.LabCommands.Exceptions
{
    public class LabCommandOrchestrationDependencyException : Xeption
    {
        public LabCommandOrchestrationDependencyException(Xeption innerException)
            : base(message: "Lab command orchestration dependency error occured. Please fix and try again.",
                  innerException)
        { }
    }
}
