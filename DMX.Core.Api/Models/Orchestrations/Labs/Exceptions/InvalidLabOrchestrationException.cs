// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Orchestrations.Labs.Exceptions
{
    public class InvalidLabOrchestrationException : Xeption
    {
        public InvalidLabOrchestrationException()
            : base(message: "Invalid lab orchestration error occurred. Please fix the errors and try again.")
        { }
    }
}
