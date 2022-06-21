// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Labs.Exceptions
{
    public class InvalidLabException : Xeption
    {
        public InvalidLabException()
            : base("Lab is invalid")
        {
        }
    }
}
