// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Orchestrations.LabCommands.Exceptions;
using DMX.Core.Api.Models.Orchestrations.Labs.Exceptions;
using Xeptions;

namespace DMX.Core.Api.Services.Orchestrations.LabCommands
{
    public partial class LabCommandOrchestrationService
    {
        private delegate ValueTask<LabCommand> ReturningLabCommandFunc();

        private async ValueTask<LabCommand> TryCatch(ReturningLabCommandFunc returningLabCommandFunc)
        {
            try
            {
                return await returningLabCommandFunc();
            }
            catch (NullLabCommandOrchestrationException nullLabCommandOrchestrationException)
            {
                throw CreateAndLogValidationException(nullLabCommandOrchestrationException);
            }
        }

        private LabCommandOrchestrationValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var labCommandOrchestrationValidationException =
                new LabCommandOrchestrationValidationException(exception);

            this.loggingBroker.LogError(labCommandOrchestrationValidationException);

            return labCommandOrchestrationValidationException;
        }
    }
}
