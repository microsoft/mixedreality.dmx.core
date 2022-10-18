// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace DMX.Core.Api.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandService
    {
        private delegate ValueTask<LabWorkflowCommand> ReturningLabWorkflowCommandFunction();

        private async ValueTask<LabWorkflowCommand> TryCatch(ReturningLabWorkflowCommandFunction returningLabWorkflowFunction)
        {
            try
            {
                return await returningLabWorkflowFunction();
            }
            catch (NullLabWorkflowCommandException nullLabWorkflowCommandException)
            {
                throw CreateAndLogValidationException(nullLabWorkflowCommandException);
            }
            catch (InvalidLabWorkflowCommandException invalidLabWorkflowCommandException)
            {
                throw CreateAndLogValidationException(invalidLabWorkflowCommandException);
            }
            catch (SqlException sqlException)
            {
                var failedLabWorkflowCommandStorageException =
                    new FailedLabWorkflowCommandStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedLabWorkflowCommandStorageException);
            }
        }

        public LabWorkflowCommandValidationException CreateAndLogValidationException(Xeption exception)
        {
            var labWorfklowCommandValidationException = 
                new LabWorkflowCommandValidationException(exception);

            this.loggingBroker.LogError(labWorfklowCommandValidationException);

            return labWorfklowCommandValidationException;
        }

        private LabWorkflowCommandDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var labWorkflowCommandDependencyException =
                new LabWorkflowCommandDependencyException(exception);

            this.loggingBroker.LogCritical(labWorkflowCommandDependencyException);

            return labWorkflowCommandDependencyException;
        }
    }
}
