// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Foundations.Labs.Exceptions;

namespace DMX.Core.Api.Services.Orchestrations
{
    public partial class LabOrchestrationService
    {
        private void ValidateLabOnAdd(Lab lab) =>
            ValidateLabIsNotNull(lab);
        
        private static void ValidateLabIdIsNotEmpty(Guid labId)
        {
            if (labId == Guid.Empty)
            {
                throw new InvalidLabIdException(labId);
            }
        }

        private static void ValidateLabIsNotNull(Lab lab)
        {
            if (lab is null)
            {
                throw new NullLabException();
            }
        }
    }
}
