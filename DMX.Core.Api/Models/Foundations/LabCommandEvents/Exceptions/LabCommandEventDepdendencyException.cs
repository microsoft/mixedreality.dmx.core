// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions
{
    public class LabCommandEventDependencyException : Xeption
    {
        public LabCommandEventDependencyException(Xeption innerException)
            : base(message: "Lab command event dependency error occurred, please contact support.",
                  innerException)
        { }
    }
}
