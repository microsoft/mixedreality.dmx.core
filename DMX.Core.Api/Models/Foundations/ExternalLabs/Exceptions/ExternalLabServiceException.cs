// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.ExternalLabs.Exceptions
{
    public class ExternalLabServiceException : Xeption
    {
        public ExternalLabServiceException(Xeption innerException)
            : base(message: "External lab service error occurred, contact support.",
                  innerException)
        { }
    }
}
