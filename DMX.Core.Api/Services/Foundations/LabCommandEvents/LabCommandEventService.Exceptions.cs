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
    }
}
