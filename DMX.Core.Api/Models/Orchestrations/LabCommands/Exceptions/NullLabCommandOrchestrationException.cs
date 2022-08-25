// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.LabCommands.Exceptions
{
    public class NullLabCommandOrchestrationException : Xeption
    {
        public NullLabCommandOrchestrationException()
            : base(message: "Lab command is null.")
        { }
    }
}
