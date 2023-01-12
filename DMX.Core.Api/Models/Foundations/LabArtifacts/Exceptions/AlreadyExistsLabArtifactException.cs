// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabArtifacts.Exceptions
{
    public class AlreadyExistsLabArtifactException : Xeption
    {
        public AlreadyExistsLabArtifactException(Exception innerException)
            : base(message: "Lab artifact with same name already exists.",
                  innerException)
        { }
    }
}
