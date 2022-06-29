// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.Labs.Exceptions
{
    public class InvalidLabException : Xeption
    {
        public InvalidLabException()
            : base("Invalid lab error occurred. Please fix the errors and try again.")
        {
        }
    }
}
