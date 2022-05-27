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

namespace DMX.Core.Api.Services.Foundations
{
    public partial class LabService
    {
        private delegate ValueTask<List<Lab>> ReturningLabFunction();

        private async ValueTask<List<Lab>> TryCatch(ReturningLabFunction returningLabFunction)
        {
            try
            {
                return await returningLabFunction();
            }
            catch (Exception exception) when (exception
                is HttpResponseUrlNotFoundException
                or HttpResponseUnauthorizedException
                or HttpResponseForbiddenException)
            {
                var failedLabDependencyException = new FailedLabDependencyException(exception);

                throw CreateAndLogCriticalDependencyException(failedLabDependencyException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedLabDependencyException = new FailedLabDependencyException(httpResponseException);

                throw CreateAndLogDependencyException(failedLabDependencyException);
            }
            catch (Exception exception)
            {
                var failedLabServiceException = new FailedLabServiceException(exception);

                throw CreateAndLogServiceException(failedLabServiceException);
            }
        }

        private LabDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var labDependencyException = new LabDependencyException(exception);
            this.loggingBroker.LogCritical(labDependencyException);

            return labDependencyException;
        }

        private LabDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var labDependencyException = new LabDependencyException(exception);
            this.loggingBroker.LogError(labDependencyException);

            return labDependencyException;
        }

        private LabServiceException CreateAndLogServiceException(Xeption exception)
        {
            var labServiceException = new LabServiceException(exception);
            this.loggingBroker.LogError(labServiceException);

            return labServiceException;
        }
    }
}
