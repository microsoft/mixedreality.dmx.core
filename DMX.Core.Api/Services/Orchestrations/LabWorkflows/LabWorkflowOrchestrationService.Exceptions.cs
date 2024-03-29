﻿// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions;
using DMX.Core.Api.Models.Foundations.LabWorkflowEvent.Exceptions;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;
using DMX.Core.Api.Models.Orchestrations.LabWorkflows.Exceptions;
using Xeptions;

namespace DMX.Core.Api.Services.Orchestrations.LabWorkflows
{
    public partial class LabWorkflowOrchestrationService : ILabWorkflowOrchestrationService
    {
        private delegate ValueTask<LabWorkflow> ReturningLabWorkflowFunction();

        private async ValueTask<LabWorkflow> TryCatch(ReturningLabWorkflowFunction returningLabWorkflowFunction)
        {
            try
            {
                return await returningLabWorkflowFunction();
            }
            catch (NullLabWorkflowOrchestrationException nullLabWorkflowOrchestrationException)
            {
                throw CreateAndLogValidationException(nullLabWorkflowOrchestrationException);
            }
            catch (InvalidLabWorkflowOrchestrationException invalidLabWorkflowOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidLabWorkflowOrchestrationException);
            }
            catch (LabWorkflowValidationException labWorkflowValidationException)
            {
                throw CreateAndLogDependencyValidationException(labWorkflowValidationException);
            }
            catch (LabWorkflowDependencyValidationException labWorkflowDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(labWorkflowDependencyValidationException);
            }
            catch (LabWorkflowCommandValidationException labWorkflowCommandValidationException)
            {
                throw CreateAndLogDependencyValidationException(labWorkflowCommandValidationException);
            }
            catch (LabWorkflowCommandDependencyValidationException labWorkflowCommandDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(labWorkflowCommandDependencyValidationException);
            }
            catch (LabWorkflowEventValidationException labWorkflowEventValidationException)
            {
                throw CreateAndLogDependencyValidationException(labWorkflowEventValidationException);
            }
            catch (LabWorkflowEventDependencyValidationException labWorkflowEventDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(labWorkflowEventDependencyValidationException);
            }
            catch (LabWorkflowDependencyException labWorkflowDependencyException)
            {
                throw CreateAndLogDependencyException(labWorkflowDependencyException);
            }
            catch (LabWorkflowServiceException labWorkflowServiceException)
            {
                throw CreateAndLogDependencyException(labWorkflowServiceException);
            }
            catch (LabWorkflowCommandDependencyException labWorkflowCommandDependencyException)
            {
                throw CreateAndLogDependencyException(labWorkflowCommandDependencyException);
            }
            catch (LabWorkflowCommandServiceException labWorkflowCommandServiceException)
            {
                throw CreateAndLogDependencyException(labWorkflowCommandServiceException);
            }
            catch (LabWorkflowEventDependencyException labWorkflowEventDependencyException)
            {
                throw CreateAndLogDependencyException(labWorkflowEventDependencyException);
            }
            catch (LabWorkflowEventServiceException labWorkflowEventServiceException)
            {
                throw CreateAndLogDependencyException(labWorkflowEventServiceException);
            }
            catch (Exception serviceException)
            {
                var failedLabWorkflowOrchestrationServiceException =
                    new FailedLabWorkflowOrchestrationServiceException(serviceException);

                throw CreateAndLogServiceException(failedLabWorkflowOrchestrationServiceException);
            }
        }

        private LabWorkflowOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var labWorkflowOrchestrationValidationException =
                new LabWorkflowOrchestrationValidationException(exception);

            this.loggingBroker.LogError(labWorkflowOrchestrationValidationException);

            return labWorkflowOrchestrationValidationException;
        }

        private LabWorkflowOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var labWorkflowOrchestrationDependencyValidationException =
                new LabWorkflowOrchestrationDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(labWorkflowOrchestrationDependencyValidationException);

            return labWorkflowOrchestrationDependencyValidationException;
        }

        private LabWorkflowOrchestrationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var labWorkflowOrchestrationDependencyException =
                new LabWorkflowOrchestrationDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(labWorkflowOrchestrationDependencyException);

            return labWorkflowOrchestrationDependencyException;
        }

        private LabWorkflowOrchestrationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var labWorkflowOrchestrationServiceException =
                new LabWorkflowOrchestrationServiceException(exception);

            this.loggingBroker.LogError(labWorkflowOrchestrationServiceException);

            return labWorkflowOrchestrationServiceException;
        }
    }
}
