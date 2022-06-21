﻿// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using Xeptions;

namespace DMX.Core.Api.Services.Foundations.Labs
{
    public partial class LabService
    {
        private delegate ValueTask<Lab> ReturningLabFunction();

        private async ValueTask<Lab> TryCatch(ReturningLabFunction returningLabFunction)
        {
            try
            {
                return await returningLabFunction();
            }
            catch (NullLabException nullLabException)
            {
                throw CreateAndLogValidationException(nullLabException);
            }
        }

        private LabValidationException CreateAndLogValidationException(Xeption nullLabException)
        {
            var labValidationException = new LabValidationException(nullLabException);
            this.loggingBroker.LogError(exception: labValidationException);

            return labValidationException;
        }
    }
}
