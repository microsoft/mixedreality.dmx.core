// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommands.Exceptions
{
    public class LabCommandServiceException : Xeption
    {
        public LabCommandServiceException(Xeption exception)
            : base(message: "Lab Command service error occurred. Contact support.",
                  exception)
        { }
    }
}
