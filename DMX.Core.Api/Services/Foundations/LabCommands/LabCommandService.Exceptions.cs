// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using Xeptions;

namespace DMX.Core.Api.Services.Foundations.LabCommands
{
    public partial class LabCommandService : ILabCommandService
    {
        private delegate ValueTask<LabCommand> ReturningLabCommandFunction();

        private async ValueTask<LabCommand> TryCatch(ReturningLabCommandFunction returningLabCommandFunction)
        {
            try
            {
                return await returningLabCommandFunction();
            }
            catch (InvalidLabCommandException invalidLabCommandException)
            {
                throw CreateAndLogValidationException(invalidLabCommandException);
            }
        }

        private LabCommandValidationException CreateAndLogValidationException(Xeption exception)
        {
            var labCommandValidationException = new LabCommandValidationException(exception);
            this.loggingBroker.LogError(exception: labCommandValidationException);

            return labCommandValidationException;
        }
    }
}
