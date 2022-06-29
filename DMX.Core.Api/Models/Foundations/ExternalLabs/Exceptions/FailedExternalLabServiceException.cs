// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.ExternalLabs.Exceptions
{
    public class FailedExternalLabServiceException : Xeption
    {
        public FailedExternalLabServiceException(Exception innerException)
            : base(message: "Failed external lab service error occurred, contact support.",
                  innerException)
        { }
    }
}
