// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.Labs.Exceptions
{
    public class NullLabOrchestrationException : Xeption
    {
        public NullLabOrchestrationException()
            : base(message: "Lab is null.")
        { }
    }
}
