// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommands.Exceptions
{
    public class LabCommandValidationException : Xeption
    {
        public LabCommandValidationException(Xeption innerException)
            : base(message: "Lab command validation error occurred, please contact support.",
                  innerException)
        { }
    }
}
