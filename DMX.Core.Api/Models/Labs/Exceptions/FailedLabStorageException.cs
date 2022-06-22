// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Labs.Exceptions
{
    public class FailedLabStorageException : Xeption
    {
        public FailedLabStorageException(Exception innerException)
            : base(message: "Failed lab storage error occurred, please contact support.",
                  innerException)
        { }
    }
}
