// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowEvent.Exceptions;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;
using Microsoft.Azure.ServiceBus;
using Xeptions;

namespace DMX.Core.Api.Services.Foundations.LabWorkflowEvents
{
    public partial class LabWorkflowEventService
    {
        private delegate ValueTask<LabWorkflow> ReturningLabWorkflowFunction();

        private async ValueTask<LabWorkflow> TryCatch(ReturningLabWorkflowFunction returningLabWorkflowFunction)
        {
            try
            {
                return await returningLabWorkflowFunction();
            }
            catch(NullLabWorkflowException nullLabWorkflowException)
            {
                throw CreateAndLogValidationException(nullLabWorkflowException);
            }
            catch(MessagingEntityNotFoundException messagingEntityNotFoundException)
            {
                var failedLabWorkflowEventDependencyException =
                    new FailedLabWorkflowEventDependencyException(messagingEntityNotFoundException);

                throw CreateAndLogCriticalDependencyException(failedLabWorkflowEventDependencyException);
            }
            catch(MessagingEntityDisabledException messagingEntityDisabledException)
            {
                var failedLabWorkflowEventDependencyException =
                    new FailedLabWorkflowEventDependencyException(messagingEntityDisabledException);

                throw CreateAndLogCriticalDependencyException(failedLabWorkflowEventDependencyException);
            }
            catch(UnauthorizedAccessException unauthorizedAccessException)
            {
                var failedLabWorkflowEventDependencyException =
                    new FailedLabWorkflowEventDependencyException(unauthorizedAccessException);

                throw CreateAndLogCriticalDependencyException(failedLabWorkflowEventDependencyException);
            }
        }

        private LabWorkflowEventValidationException CreateAndLogValidationException(NullLabWorkflowException nullLabWorkflowException)
        {
            var labWorkflowEventValidationException = new LabWorkflowEventValidationException(nullLabWorkflowException);
            this.loggingBroker.LogError(labWorkflowEventValidationException);

            return labWorkflowEventValidationException;
        }

        private LabWorkflowEventDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var labWorkflowEventDependencyException =
                new LabWorkflowEventDependencyException(exception);

            this.loggingBroker.LogCritical(labWorkflowEventDependencyException);

            return labWorkflowEventDependencyException;
        }
    }
}
