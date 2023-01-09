// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabArtifacts.Exceptions
{
    public class InvalidLabArtifactException : Xeption
    {
        public InvalidLabArtifactException() :
            base(message: "Invalid lab artifact error occurred. Please fix the errors and try again.")
        { }
    }
}
