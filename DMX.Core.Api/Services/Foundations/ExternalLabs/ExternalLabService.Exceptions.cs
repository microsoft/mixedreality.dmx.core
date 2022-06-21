// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.ExternalLabs.Exceptions;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using RESTFulSense.Exceptions;
using Xeptions;

namespace DMX.Core.Api.Services.Foundations.ExternalLabs
{
    public partial class ExternalLabService
    {
        private delegate ValueTask<List<Lab>> ReturningLabsFunction();

        private async ValueTask<List<Lab>> TryCatch(ReturningLabsFunction returningLabsFunction)
        {
            try
            {
                return await returningLabsFunction();
            }
            catch (HttpResponseUrlNotFoundException httpResponseUrlNotFoundException)
            {
                var failedExternalLabDependencyException =
                    new FailedExternalLabDependencyException(httpResponseUrlNotFoundException);

                throw CreateAndLogCriticalDependencyException(failedExternalLabDependencyException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var failedExternalLabDependencyException =
                    new FailedExternalLabDependencyException(httpResponseUnauthorizedException);

                throw CreateAndLogCriticalDependencyException(failedExternalLabDependencyException);
            }
            catch (HttpResponseForbiddenException httpResponseForbiddenException)
            {
                var failedExternalLabDependencyException =
                    new FailedExternalLabDependencyException(httpResponseForbiddenException);

                throw CreateAndLogCriticalDependencyException(failedExternalLabDependencyException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedExternalLabDependencyException = new FailedExternalLabDependencyException(httpResponseException);

                throw CreateAndLogDependencyException(failedExternalLabDependencyException);
            }
            catch (Exception exception)
            {
                var failedExternalLabServiceException = new FailedExternalLabServiceException(exception);

                throw CreateAndLogServiceException(failedExternalLabServiceException);
            }
        }

        private ExternalLabDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var externalLabDependencyException = new ExternalLabDependencyException(exception);
            this.loggingBroker.LogCritical(externalLabDependencyException);

            return externalLabDependencyException;
        }

        private ExternalLabDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var externalLabDependencyException = new ExternalLabDependencyException(exception);
            this.loggingBroker.LogError(externalLabDependencyException);

            return externalLabDependencyException;
        }

        private ExternaLabServiceException CreateAndLogServiceException(Xeption exception)
        {
            var externalLabServiceException = new ExternaLabServiceException(exception);
            this.loggingBroker.LogError(externalLabServiceException);

            return externalLabServiceException;
        }
    }
}
