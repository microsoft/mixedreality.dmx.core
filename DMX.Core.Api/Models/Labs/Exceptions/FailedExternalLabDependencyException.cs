// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Labs.Exceptions
{
    public class FailedExternalLabDependencyException : Xeption
    {
        public FailedExternalLabDependencyException(Exception innerException)
            : base(message: "Failed external lab dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
