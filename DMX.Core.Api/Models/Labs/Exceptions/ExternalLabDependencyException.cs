// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Labs.Exceptions
{
    public class ExternalLabDependencyException : Xeption
    {
        public ExternalLabDependencyException(Xeption innerException)
            : base(message: "External dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
