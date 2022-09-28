// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
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
        }

        private ArtifactValidationException CreateAndLogValidationException(Xeption exception)
        {
            var artifactValidationException =
                new ArtifactValidationException(exception);

            this.loggingBroker.LogError(artifactValidationException);

            return artifactValidationException;
        }
    }
}
