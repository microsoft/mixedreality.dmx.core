// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.Labs.Exceptions
{
    public class AlreadyExistsLabException : Xeption
    {
        public AlreadyExistsLabException(Exception innerException)
            : base(message: "Lab with same Id already exists",
                  innerException)
        { }
    }
}
