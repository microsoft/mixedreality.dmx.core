// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.Labs.Exceptions
{
    public class LockedLabException : Xeption
    {
        public LockedLabException(Exception innerException)
            : base(message: "Locked lab record error occurred. Please try again later.", innerException)
        { }
    }
}
