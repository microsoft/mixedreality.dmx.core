// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommands.Exceptions
{
    public class InvalidLabCommandException : Xeption
    {
        public InvalidLabCommandException()
            : base(message: "Lab command is is invalid. Please fix and try again.")
        { }
    }
}
