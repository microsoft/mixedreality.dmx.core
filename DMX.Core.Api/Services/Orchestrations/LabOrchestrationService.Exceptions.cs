// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.ExternalLabs.Exceptions;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Foundations.Labs.Exceptions;
using DMX.Core.Api.Models.Orchestrations.Labs.Exceptions;
using Microsoft.AspNetCore.Server.IIS.Core;
using Xeptions;

namespace DMX.Core.Api.Services.Orchestrations
{
    public partial class LabOrchestrationService
    {
        private delegate ValueTask<List<Lab>> ReturningLabsFunction();
        private delegate ValueTask<Lab> ReturningLabFunction();

        private async ValueTask<List<Lab>> TryCatch(ReturningLabsFunction returningLabsFunction)
        {
            try
            {
                return await returningLabsFunction();
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
            catch (Exception exception)
            {
                var failedLabOrchestrationServiceException =
                    new FailedLabOrchestrationServiceException(exception);

                throw CreateAndLogServiceException(failedLabOrchestrationServiceException);
            }
        }

        private async ValueTask<Lab> TryCatch(ReturningLabFunction returningLabFunction)
        {
            try
            {
                return await returningLabFunction();
            }
            catch (NullLabException nullLabException)
            {
                throw CreateAndLogValidationException(nullLabException);
            }
            catch (InvalidLabException invalidLabException)
            {
                throw CreateAndLogValidationException(invalidLabException);
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
            catch (Exception exception)
            {
                var failedLabOrchestrationServiceException =
                    new FailedLabOrchestrationServiceException(exception);

                throw CreateAndLogServiceException(failedLabOrchestrationServiceException);
            }
        }

        private LabOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var labOrchestrationValidationException =
                new LabOrchestrationValidationException(exception);

            this.loggingBroker.LogError(exception: labOrchestrationValidationException);

            return labOrchestrationValidationException;
        }

        private LabOrchestrationDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var orchestrationDependencyException =
                new LabOrchestrationDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(orchestrationDependencyException);

            return orchestrationDependencyException;
        }

        private LabOrchestrationServiceException CreateAndLogServiceException(
            FailedLabOrchestrationServiceException failedLabOrchestrationServiceException)
        {
            var labOrchestrationServiceException =
                new LabOrchestrationServiceException(
                    failedLabOrchestrationServiceException);

            this.loggingBroker.LogError(labOrchestrationServiceException);

            return labOrchestrationServiceException;
        }
    }
}
