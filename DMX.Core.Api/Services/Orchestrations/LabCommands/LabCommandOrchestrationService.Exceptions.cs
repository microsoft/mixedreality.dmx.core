// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions;
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
            catch (LabCommandEventValidationException labCommandEventValidationException)
            {
                throw CreateAndLogDependencyValidationException(labCommandEventValidationException);
            }
            catch (LabCommandDependencyValidationException labCommandDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(labCommandDependencyValidationException);
            }
            catch (LabCommandEventDependencyValidationException labCommandEventDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(labCommandEventDependencyValidationException);
            }
            catch (LabCommandDependencyException labCommandDependencyException)
            {
                throw CreateAndLogDependencyException(labCommandDependencyException);
            }
            catch (LabCommandServiceException labCommandServiceException)
            {
                throw CreateAndLogDependencyException(labCommandServiceException);
            }
            catch (Exception exception)
            {
                var failedLabCommandOrchestrationServiceException =
                    new FailedLabCommandOrchestrationServiceException(exception);

                throw CreateAndLogServiceException(failedLabCommandOrchestrationServiceException);
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

        private LabCommandOrchestrationDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var labCommandOrchestrationDependencyException =
                new LabCommandOrchestrationDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(labCommandOrchestrationDependencyException);

            return labCommandOrchestrationDependencyException;
        }

        private LabCommandOrchestrationServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var labCommandOrchestrationServiceException =
                new LabCommandOrchestrationServiceException(exception);

            this.loggingBroker.LogError(labCommandOrchestrationServiceException);

            return labCommandOrchestrationServiceException;
        }
    }
}
