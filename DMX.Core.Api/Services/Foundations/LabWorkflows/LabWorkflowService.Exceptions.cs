// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace DMX.Core.Api.Services.Foundations.LabWorkflows
{
    public partial class LabWorkflowService
    {
        private delegate ValueTask<LabWorkflow> ReturningLabWorkflowFunction();

        private async ValueTask<LabWorkflow> TryCatch(ReturningLabWorkflowFunction returningLabWorkflowFunction)
        {
            try
            {
                return await returningLabWorkflowFunction();
            }
            catch (InvalidLabWorkflowException invalidLabWorkflowException)
            {
                throw CreateAndLogValidationException(invalidLabWorkflowException);
            }
            catch (NotFoundLabWorkflowException notFoundLabWorkflowException)
            {
                throw CreateAndLogValidationException(notFoundLabWorkflowException);
            }
            catch (SqlException sqlException)
            {
                var failedLabWorkflowStorageException =
                    new FailedLabWorkflowStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedLabWorkflowStorageException);
            }
        }

        private LabWorkflowValidationException CreateAndLogValidationException(Xeption exception)
        {
            var labWorkflowValidationException =
                new LabWorkflowValidationException(exception);

            this.loggingBroker.LogError(labWorkflowValidationException);

            return labWorkflowValidationException;
        }

        private LabWorkflowDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var labWorkflowDependencyException =
                new LabWorkflowDependencyException(exception);

            this.loggingBroker.LogCritical(labWorkflowDependencyException);

            return labWorkflowDependencyException;
        }
    }
}
