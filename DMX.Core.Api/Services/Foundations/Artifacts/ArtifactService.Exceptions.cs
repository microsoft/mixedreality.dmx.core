// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Azure;
using DMX.Core.Api.Models.Foundations.Artifacts.Exceptions;
using Xeptions;

namespace DMX.Core.Api.Services.Foundations.Artifacts
{
    public partial class ArtifactService
    {
        private delegate ValueTask ReturningFunction();

        private async ValueTask TryCatch(ReturningFunction returningFunction)
        {
            try
            {
                await returningFunction();
            }
            catch (NullArtifactException nullArtifactException)
            {
                throw CreateAndLogValidationException(nullArtifactException);
            }
            catch (InvalidArtifactException invalidArtifactException)
            {
                throw CreateAndLogValidationException(invalidArtifactException);
            }
            catch (RequestFailedException requestFailedException)
            {
                var failedArtifactDependencyException =
                    new FailedArtifactDependencyException(requestFailedException);

                throw CreateAndLogDependencyException(failedArtifactDependencyException);
            }
            catch (Exception exception)
            {
                var failedArtifactServiceException =
                    new FailedArtifactServiceException(exception);

                throw CreateAndLogServiceException(failedArtifactServiceException);
            }
        }

        private ArtifactValidationException CreateAndLogValidationException(Xeption exception)
        {
            var artifactValidationException =
                new ArtifactValidationException(exception);

            this.loggingBroker.LogError(artifactValidationException);

            return artifactValidationException;
        }

        private ArtifactDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var artifactDependencyException =
                new ArtifactDependencyException(exception);

            this.loggingBroker.LogError(artifactDependencyException);

            return artifactDependencyException;
        }

        private ArtifactServiceException CreateAndLogServiceException(Xeption exception)
        {
            var artifactServiceException =
                new ArtifactServiceException(exception);

            this.loggingBroker.LogError(artifactServiceException);

            return artifactServiceException;
        }
    }
}
