// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommands.Exceptions
{
    public class NotFoundLabCommandException : Xeption
    {
        public NotFoundLabCommandException(Guid labCommandId)
            : base(message: $"Could not find lab command with id {labCommandId}")
        { }
    }
}
