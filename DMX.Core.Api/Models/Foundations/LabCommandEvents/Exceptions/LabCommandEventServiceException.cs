// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions
{
    public class LabCommandEventServiceException : Xeption
    {
        public LabCommandEventServiceException(Xeption innerException)
            : base(message: "Lab command event service error occurred, please contact support.",
                   innerException)
        { }
    }
}
