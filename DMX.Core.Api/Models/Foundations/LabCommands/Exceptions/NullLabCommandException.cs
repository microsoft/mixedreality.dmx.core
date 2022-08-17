// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommands.Exceptions
{
    public class NullLabCommandException : Xeption
    {
        public NullLabCommandException()
            : base(message: "Lab command is null.")
        { }
    }
}
