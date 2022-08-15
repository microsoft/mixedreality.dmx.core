// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using DMX.Core.Api.Models.Foundations.Labs.Exceptions;
using Xeptions;

namespace DMX.Core.Api.Services.Foundations.LabCommands
{
    public partial class LabCommandService
    {
        private delegate ValueTask<LabCommand> ReturningLabCommandFunction();

        private async ValueTask<LabCommand> TryCatch(
            ReturningLabCommandFunction returningLabCommandFunction)
        {
            try
            {
                return await returningLabCommandFunction();
            }
            catch (NullLabCommandException nullLabCommandException)
            {
                throw CreateAndLogValidationException(nullLabCommandException);
            }
            catch (InvalidLabCommandException invalidLabCommandException)
            {
                throw CreateAndLogValidationException(invalidLabCommandException);
            }
        }

        private Xeption CreateAndLogValidationException(Xeption exception)
        {
            var labCommandValidationException =
                new LabCommandValidationException(exception);

            this.loggingBroker.LogError(labCommandValidationException);

            return labCommandValidationException;
        }
    }
}