// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Labs.Exceptions
{
    public class LabDependencyException : Xeption
    {
        public LabDependencyException(Xeption innerException)
            : base(message: "Lab dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
