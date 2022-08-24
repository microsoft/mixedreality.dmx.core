// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions
{
    public class FailedLabCommandEventServiceException : Xeption
    {
        public FailedLabCommandEventServiceException(Exception innerException)
            : base(message: "Lab command event service error occured. Please fix and try again.")
        { }
    }
}
