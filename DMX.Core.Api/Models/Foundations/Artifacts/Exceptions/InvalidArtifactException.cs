// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace DMX.Core.Api.Models.Foundations.Artifacts.Exceptions
{
    public class InvalidArtifactException : Xeption
    {
        public InvalidArtifactException() :
            base(message: "Invalid artifact error occurred. Please fix the errors and try again.")
        { }
    }
}
