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
using AzureMessagingCommunicationException = Microsoft.ServiceBus.Messaging.MessagingCommunicationException;



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
            catch (NullLabWorkflowException nullLabWorkflowException)
            {
                throw CreateAndLogValidationException(nullLabWorkflowException);
            }
            catch (MessagingEntityNotFoundException messagingEntityNotFoundException)
            {
                var failedLabWorkflowEventDependencyException =
                    new FailedLabWorkflowEventDependencyException(messagingEntityNotFoundException);

                throw CreateAndLogCriticalDependencyException(failedLabWorkflowEventDependencyException);
            }
            catch (MessagingEntityDisabledException messagingEntityDisabledException)
            {
                var failedLabWorkflowEventDependencyException =
                    new FailedLabWorkflowEventDependencyException(messagingEntityDisabledException);

                throw CreateAndLogCriticalDependencyException(failedLabWorkflowEventDependencyException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                var failedLabWorkflowEventDependencyException =
                    new FailedLabWorkflowEventDependencyException(unauthorizedAccessException);

                throw CreateAndLogCriticalDependencyException(failedLabWorkflowEventDependencyException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                var failedLabWorkflowEventDependencyException = new FailedLabWorkflowEventDependencyException(invalidOperationException);

                throw CreateAndLogDependencyException(failedLabWorkflowEventDependencyException);
            }
            catch (AzureMessagingCommunicationException azureMessagingCommunicationException)
            {
                var failedLabWorkflowEventDependencyException = new FailedLabWorkflowEventDependencyException(azureMessagingCommunicationException);

                throw CreateAndLogDependencyException(failedLabWorkflowEventDependencyException);
            }
            catch (ServerBusyException serverBusyException)
            {
                var failedLabWorkflowEventDependencyException = new FailedLabWorkflowEventDependencyException(serverBusyException);

                throw CreateAndLogDependencyException(failedLabWorkflowEventDependencyException);
            }
            catch (ArgumentException argumentException)
            {
                var invalidLabWorkflowEventException =
                    new InvalidLabWorkflowEventException(argumentException);

                throw CreateAndLogDependencyValidationException(invalidLabWorkflowEventException);
            }
            catch (Exception exception)
            {
                var failedLabWorkflowEventServiceException =
                    new FailedLabWorkflowEventServiceException(exception);

                throw CreateAndLogServiceException(failedLabWorkflowEventServiceException);
            }
        }

        private LabWorkflowEventServiceException CreateAndLogServiceException(Xeption exception)
        {
            var labWorkflowEventServiceException = new LabWorkflowEventServiceException(exception);
            this.loggingBroker.LogError(labWorkflowEventServiceException);

            return labWorkflowEventServiceException;
        }

        private LabWorkflowEventDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var labWorkflowEventDependencyException = new LabWorkflowEventDependencyException(exception);
            this.loggingBroker.LogError(labWorkflowEventDependencyException);

            return labWorkflowEventDependencyException;
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

        private LabWorkflowEventDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var labWorkflowEventDependencyValidationException =
                new LabWorkflowEventDependencyValidationException(exception);

            this.loggingBroker.LogError(labWorkflowEventDependencyValidationException);

            return labWorkflowEventDependencyValidationException;

        }
    }
}
