// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Net;
using System.Threading.Tasks;
using Azure;
using DMX.Core.Api.Models.Foundations.LabArtifacts.Exceptions;
using Xeptions;

namespace DMX.Core.Api.Services.Foundations.LabArtifacts
{
    public partial class LabArtifactService
    {
        private delegate ValueTask ReturningFunction();

        private async ValueTask TryCatch(ReturningFunction returningFunction)
        {
            try
            {
                await returningFunction();
            }
            catch (NullLabArtifactException nullLabArtifactException)
            {
                throw CreateAndLogValidationException(nullLabArtifactException);
            }
            catch (InvalidLabArtifactException invalidLabArtifactException)
            {
                throw CreateAndLogValidationException(invalidLabArtifactException);
            }
            catch (RequestFailedException requestFailedException)
                when (requestFailedException.Status 
                    is (int)HttpStatusCode.Unauthorized
                    or (int)HttpStatusCode.Forbidden
                    or (int)HttpStatusCode.NotFound)
            {
                var failedLabArtifactDependencyException =
                    new FailedLabArtifactDependencyException(requestFailedException);

                throw CreateAndLogCriticalDependencyException(failedLabArtifactDependencyException);
            }
            catch (RequestFailedException requestFailedException)
            {
                var failedLabArtifactDependencyException =
                    new FailedLabArtifactDependencyException(requestFailedException);

                throw CreateAndLogDependencyException(failedLabArtifactDependencyException);
            }
            catch (Exception exception)
            {
                var failedLabArtifactServiceException =
                    new FailedLabArtifactServiceException(exception);

                throw CreateAndLogServiceException(failedLabArtifactServiceException);
            }
        }

        private LabArtifactValidationException CreateAndLogValidationException(Xeption exception)
        {
            var labArtifactValidationException =
                new LabArtifactValidationException(exception);

            this.loggingBroker.LogError(labArtifactValidationException);

            return labArtifactValidationException;
        }

        private LabArtifactDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var labArtifactDependencyException =
                new LabArtifactDependencyException(exception);

            this.loggingBroker.LogCritical(labArtifactDependencyException);
            
            return labArtifactDependencyException;
        }

        private LabArtifactDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var labArtifactDependencyException =
                new LabArtifactDependencyException(exception);

            this.loggingBroker.LogError(labArtifactDependencyException);

            return labArtifactDependencyException;
        }

        private LabArtifactServiceException CreateAndLogServiceException(Xeption exception)
        {
            var labArtifactServiceException =
                new LabArtifactServiceException(exception);

            this.loggingBroker.LogError(labArtifactServiceException);

            return labArtifactServiceException;
        }
    }
}
