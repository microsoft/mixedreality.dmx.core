// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions
{
    public class InvalidLabCommandEventArgumentException : Xeption
    {
        public InvalidLabCommandEventArgumentException(Exception innerException)
            : base(message: "Invalid lab command event argument exception occurred, please contact support.",
                  innerException)
        { }
    }
}
