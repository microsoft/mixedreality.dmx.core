// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using Xeptions;

namespace DMX.Core.Api.Services.Foundations.LabCommandEvents
{
    public partial class LabCommandEventService
    {
        private delegate ValueTask<LabCommand> ReturningLabCommandFunction();

        private async ValueTask<LabCommand> TryCatch(ReturningLabCommandFunction returningLabCommandFunction)
        {
            try
            {
                return await returningLabCommandFunction();
            }
            catch (NullLabCommandException nullLabCommandException)
            {
                throw CreateAndLogValidationException(nullLabCommandException);
            }
        }

        private LabCommandEventValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var labCommandEventValidationException =
                new LabCommandEventValidationException(exception);

            loggingBroker.LogError(labCommandEventValidationException);

            return labCommandEventValidationException;
        }
    }
}
