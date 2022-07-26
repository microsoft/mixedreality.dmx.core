// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.Labs.Exceptions
{
    public class InvalidLabIdException : Xeption
    {
        public InvalidLabIdException(Guid labId)
            : base(message: $"Lab Id '{labId}' is invalid. Please use new Id and try again")
        { }
    }
}
