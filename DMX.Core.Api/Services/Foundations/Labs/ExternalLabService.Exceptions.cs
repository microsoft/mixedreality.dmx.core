// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using RESTFulSense.Exceptions;
using Xeptions;

namespace DMX.Core.Api.Services.Foundations.Labs
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
                var failedLabDependencyException =
                    new FailedExternalLabDependencyException(httpResponseUrlNotFoundException);

                throw CreateAndLogCriticalDependencyException(failedLabDependencyException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var failedLabDependencyException =
                    new FailedExternalLabDependencyException(httpResponseUnauthorizedException);

                throw CreateAndLogCriticalDependencyException(failedLabDependencyException);
            }
            catch (HttpResponseForbiddenException httpResponseForbiddenException)
            {
                var failedLabDependencyException =
                    new FailedExternalLabDependencyException(httpResponseForbiddenException);

                throw CreateAndLogCriticalDependencyException(failedLabDependencyException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedLabDependencyException = new FailedExternalLabDependencyException(httpResponseException);

                throw CreateAndLogDependencyException(failedLabDependencyException);
            }
            catch (Exception exception)
            {
                var failedLabServiceException = new FailedExternalLabServiceException(exception);

                throw CreateAndLogServiceException(failedLabServiceException);
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
            var ExternalLabServiceException = new ExternaLabServiceException(exception);
            this.loggingBroker.LogError(ExternalLabServiceException);

            return ExternalLabServiceException;
        }
    }
}
