// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommands.Exceptions
{
    public class FailedLabCommandServiceException : Xeption
    {
        public FailedLabCommandServiceException(Exception exception)
            : base(message: "Failed Lab command service error occurred. Please contact support.",
                  exception)
        { }
    }
}
