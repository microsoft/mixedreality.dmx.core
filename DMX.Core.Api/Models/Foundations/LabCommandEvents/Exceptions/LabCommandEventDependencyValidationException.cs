// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions
{
    public class LabCommandEventDependencyValidationException : Xeption
    {
        public LabCommandEventDependencyValidationException(Xeption innerException)
            : base(message: "Lab command event dependency validation error occurred, please contact support",
                   innerException)
        { }
    }
}
