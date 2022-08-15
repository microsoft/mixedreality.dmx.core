// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommands.Exceptions
{
    public class FailedLabCommandStorageException : Xeption
    {
        public FailedLabCommandStorageException(Exception exception)
            : base(message: "Failed lab command storage error occured. Please contact support.", exception)
        { }
    }
}
