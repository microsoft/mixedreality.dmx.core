// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using DMX.Core.Api.Models.Orchestrations.LabCommands.Exceptions;
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
            catch (LabCommandValidationException labCommandValidationException)
            {
                throw CreateAndLogDependencyValidationException(labCommandValidationException);
            }
            catch (LabCommandDependencyValidationException labCommandDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(labCommandDependencyValidationException);
            }
            catch (LabCommandDependencyException labCommandDependencyException)
            {
                throw CreateAndLogOrchestrationDependencyException(labCommandDependencyException);
            }
            catch (LabCommandServiceException labCommandServiceException)
            {
                throw CreateAndLogOrchestrationDependencyException(labCommandServiceException);
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

        private LabCommandOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var labCommandOrchestrationDependencyValidationException =
                new LabCommandOrchestrationDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(labCommandOrchestrationDependencyValidationException);

            return labCommandOrchestrationDependencyValidationException;
        }

        private LabCommandOrchestrationDependencyException CreateAndLogOrchestrationDependencyException(
            Xeption exception)
        {
            var labCommandOrchestrationDependencyException =
                new LabCommandOrchestrationDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(labCommandOrchestrationDependencyException);

            return labCommandOrchestrationDependencyException;
        }
    }
}
