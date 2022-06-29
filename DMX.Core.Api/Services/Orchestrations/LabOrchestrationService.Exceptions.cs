// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.ExternalLabs.Exceptions;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using DMX.Core.Api.Models.Orchestrations.Labs.Exceptions;
using Xeptions;

namespace DMX.Core.Api.Services.Orchestrations
{
    public partial class LabOrchestrationService
    {
        private delegate ValueTask<List<Lab>> ReturningLabsFunctions();

        private async ValueTask<List<Lab>> TryCatch(ReturningLabsFunctions returningLabsFunctions)
        {
            try
            {
                return await returningLabsFunctions();
            }
            catch (ExternalLabDependencyException externalLabDependencyException)
            {
                throw CreateAndLogDependencyException(externalLabDependencyException);
            }
            catch (ExternalLabServiceException externalLabServiceException)
            {
                throw CreateAndLogDependencyException(externalLabServiceException);
            }
            catch (LabDependencyException labDependencyException)
            {
                throw CreateAndLogDependencyException(labDependencyException);
            }
            catch (LabServiceException labServiceException)
            {
                throw CreateAndLogDependencyException(labServiceException);
            }
        }

        private LabOrchestrationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var orchestrationDependencyException =
                new LabOrchestrationDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(orchestrationDependencyException);

            return orchestrationDependencyException;
        }
    }
}
