// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommands.Exceptions
{
    public class FailedLabCommandStorageException : Xeption
    {
        public FailedLabCommandStorageException(Exception innerException)
            : base(message: "Failed lab command storage error occurred, please contact support.",
                  innerException)
        { }
    }
}
