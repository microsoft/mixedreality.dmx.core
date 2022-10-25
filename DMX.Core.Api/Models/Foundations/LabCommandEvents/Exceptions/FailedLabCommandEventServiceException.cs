// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions
{
    public class FailedLabCommandEventServiceException : Xeption
    {
        public FailedLabCommandEventServiceException()
            : base(message: "Lab command event service error occurred. Please fix and try again.")
        { }
    }
}
