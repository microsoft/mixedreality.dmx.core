// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;

namespace DMX.Core.Api.Services.Foundations.Labs
{
    public partial class LabService
    {
        private void ValidateLabOnAdd(Lab lab)
        {
            ValidateLabIsNotNull(lab);
        }

        private void ValidateLabIsNotNull(Lab lab)
        {
            if(lab is null)
            {
                throw new NullLabException();
            }
        }
    }
}
