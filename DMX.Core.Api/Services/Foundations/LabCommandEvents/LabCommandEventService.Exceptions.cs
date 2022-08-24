// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using Microsoft.Azure.ServiceBus;
using Xeptions;
using AzureMessagingCommunicationException = Microsoft.ServiceBus.Messaging.MessagingCommunicationException;

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
            catch (MessagingEntityNotFoundException messagingEntityNotFoundException)
            {
                var failedLabCommandEventDependencyException =
                    new FailedLabCommandEventDependencyException(messagingEntityNotFoundException);

                throw CreateAndLogCriticalDependencyException(failedLabCommandEventDependencyException);
            }
            catch (MessagingEntityDisabledException messagingEntityDisabledException)
            {
                var failedLabCommandEventDependencyException =
                    new FailedLabCommandEventDependencyException(messagingEntityDisabledException);

                throw CreateAndLogCriticalDependencyException(failedLabCommandEventDependencyException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                var failedLabCommandEventDependencyException =
                    new FailedLabCommandEventDependencyException(unauthorizedAccessException);

                throw CreateAndLogCriticalDependencyException(failedLabCommandEventDependencyException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                var failedLabCommandEventDependencyException =
                    new FailedLabCommandEventDependencyException(invalidOperationException);

                throw CreateAndLogDependencyException(failedLabCommandEventDependencyException);
            }
            catch (AzureMessagingCommunicationException azureMessagingCommunicationException)
            {
                var failedLabCommandEventDependencyException =
                    new FailedLabCommandEventDependencyException(azureMessagingCommunicationException);

                throw CreateAndLogDependencyException(failedLabCommandEventDependencyException);
            }
            catch (ServerBusyException serverBusyException)
            {
                var failedLabCommandEventDependencyException =
                    new FailedLabCommandEventDependencyException(serverBusyException);

                throw CreateAndLogDependencyException(failedLabCommandEventDependencyException);
            }
            catch (ArgumentException argumentException)
            {
                var invalidLabCommandEventArgumentException =
                    new InvalidLabCommandEventArgumentException(argumentException);

                throw CreateAndLogDependencyValidationException(invalidLabCommandEventArgumentException);
            }
            catch (Exception exception)
            {
                var failedLabCommandEventServiceException =
                    new FailedLabCommandEventServiceException(exception);

                throw CreateAndLogServiceException(failedLabCommandEventServiceException);
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

        private LabCommandEventDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {

            var labCommandEventDepdendencyException =
                new LabCommandEventDependencyException(exception);

            loggingBroker.LogCritical(labCommandEventDepdendencyException);

            return labCommandEventDepdendencyException;
        }

        private LabCommandEventDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var labCommandEventDependencyException =
                new LabCommandEventDependencyException(exception);

            loggingBroker.LogError(labCommandEventDependencyException);

            return labCommandEventDependencyException;
        }

        private LabCommandEventDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var labCommandEventDependencyValidationException =
                new LabCommandEventDependencyValidationException(exception);

            loggingBroker.LogError(labCommandEventDependencyValidationException);

            return labCommandEventDependencyValidationException;
        }

        private LabCommandEventServiceException CreateAndLogServiceException(Xeption exception)
        {
            var labCommandEventServiceException =
                new LabCommandEventServiceException(exception);

            loggingBroker.LogError(labCommandEventServiceException);

            return labCommandEventServiceException;
        }
    }
}

