using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.ExternalLabs.Exceptions;
using DMX.Core.Api.Models.Labs;
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

                throw CreateAndLogDependencyException(externalLabDependencyException.InnerException as Xeption);
            }
            catch (ExternalLabServiceException externalLabServiceException)
            {

                throw CreateAndLogDependencyException(externalLabServiceException.InnerException as Xeption);
            }
        }

        private LabOrchestrationDependencyException CreateAndLogDependencyException(Xeption xeption)
        {
            var orchestrationDependencyException = new LabOrchestrationDependencyException(xeption);
            this.loggingBroker.LogError(orchestrationDependencyException);

            return orchestrationDependencyException;
        }
    }
}
