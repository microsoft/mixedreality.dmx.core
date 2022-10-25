// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions
{
    public class LabCommandEventValidationException : Xeption
    {
        public LabCommandEventValidationException(Xeption innerException)
            : base(message: "Lab command validation exception occurred. Please fix and try again.",
                  innerException)
        { }
    }
}
