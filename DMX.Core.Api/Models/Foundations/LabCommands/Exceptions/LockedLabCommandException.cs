// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommands.Exceptions
{
    public class LockedLabCommandException : Xeption
    {
        public LockedLabCommandException(Exception innerException)
            : base(message: "Locked lab command record error occurred. Please try again later.",
                  innerException)
        { }
    }
}
