// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.ExternalLabs.Exceptions
{
    public class ExternalLabDependencyException : Xeption
    {
        public ExternalLabDependencyException(Xeption innerException)
            : base(message: "External lab dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
