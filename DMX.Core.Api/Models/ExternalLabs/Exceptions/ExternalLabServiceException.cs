﻿// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Labs.Exceptions
{
    public class ExternaLabServiceException : Xeption
    {
        public ExternaLabServiceException(Xeption innerException)
            : base(message: "External lab service error occurred, contact support.",
                  innerException)
        { }
    }
}