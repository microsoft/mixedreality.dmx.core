// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.Labs.Exceptions
{
    public class InvalidLabCommandException : Xeption
    {
        public InvalidLabCommandException()
            : base(message: "Invalid lab command error occurred. Please fix the errors and try again.")
        { }
    }
}
