// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommands.Exceptions
{
    public class AlreadyExistsLabCommandException : Xeption
    {
        public AlreadyExistsLabCommandException(Exception exception)
            : base(message: "Lab Command with same Id already exists.",
                  exception)
        {
        }
    }
}
