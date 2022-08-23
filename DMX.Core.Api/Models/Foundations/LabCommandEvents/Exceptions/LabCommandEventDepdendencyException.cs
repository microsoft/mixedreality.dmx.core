// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions
{
    public class LabCommandEventDepdendencyException : Xeption
    {
        public LabCommandEventDepdendencyException(Xeption innerException)
            : base(message: "Lab command event dependency error occurred, please contact support.",
                  innerException)
        { }
    }
}
