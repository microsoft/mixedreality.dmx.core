// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.Labs.Exceptions
{
    public class LabServiceException : Xeption
    {
        public LabServiceException(Xeption innerException)
            : base(message: "Lab service error occurred, contact support.",
                  innerException)
        { }
    }
}
