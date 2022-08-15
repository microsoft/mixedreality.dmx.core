// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommands.Exceptions
{
    public class LabCommandDependencyException : Xeption
    {
        public LabCommandDependencyException(Xeption innerException)
            : base(message: "Lab command dependency error occured. Please contact support.", innerException)
        { }
    }
}
