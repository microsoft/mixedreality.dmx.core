// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions
{
    public class NullLabCommandEventException : Xeption
    {
        public NullLabCommandEventException()
            : base(message: "Lab command is null")
        { }
    }
}
