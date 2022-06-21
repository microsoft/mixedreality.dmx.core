// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using System;
using System.Threading.Tasks;
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
            catch (InvalidLabException invalidLabException)
            {
                throw CreateAndLogValidationException(invalidLabException);
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
