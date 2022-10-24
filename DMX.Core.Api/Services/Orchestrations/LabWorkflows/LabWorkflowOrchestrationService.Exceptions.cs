// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;
using DMX.Core.Api.Models.Orchestrations.LabWorkflows;
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
            catch (LabWorkflowDependencyException labWorkflowDependencyException)
            {
                throw CreateAndLogDependencyException(labWorkflowDependencyException);
            }
            catch (LabWorkflowServiceException labWorkflowServiceException)
            {
                throw CreateAndLogDependencyException(labWorkflowServiceException);
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
    }
}
