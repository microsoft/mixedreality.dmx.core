// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommands.Exceptions
{
    public class LabCommandDependencyValidationException : Xeption
    {
        public LabCommandDependencyValidationException(Xeption exception)
            : base(message: "Lab command dependency validation error occurred. Contact support",
                  exception)
        { }
    }
}
