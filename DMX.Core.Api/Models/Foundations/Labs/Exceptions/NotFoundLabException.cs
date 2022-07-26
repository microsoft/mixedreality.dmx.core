// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.Labs.Exceptions
{
    public class NotFoundLabException : Xeption
    {
        public NotFoundLabException(Guid labId)
            : base(message: $"Could not find lab with id {labId}")
        { }
    }
}
