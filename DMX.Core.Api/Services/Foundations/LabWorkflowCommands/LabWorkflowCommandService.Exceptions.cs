// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace DMX.Core.Api.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandService
    {
        public delegate ValueTask<LabWorkflowCommand> ReturningLabWorkflowCommandFunction();

        public async ValueTask<LabWorkflowCommand> TryCatch(ReturningLabWorkflowCommandFunction returningLabWorkflowCommandFunction)
        {
            try
            {
                return await returningLabWorkflowCommandFunction();
            }
            catch (NullLabWorkflowCommandException nullLabWorkflowCommandException)
            {
                throw CreateAndLogValidationException(nullLabWorkflowCommandException);
            }
            catch (InvalidLabWorkflowCommandException invalidLabWorkflowCommandException)
            {
                throw CreateAndLogValidationException(invalidLabWorkflowCommandException);
            }
            catch (NotFoundLabWorkflowCommandException notFoundLabWorkflowCommandException)
            {
                throw CreateAndLogValidationException(notFoundLabWorkflowCommandException);
            }
            catch (SqlException sqlException)
            {
                var failedLabWorkflowCommandStorageException = new FailedLabWorkflowCommandStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedLabWorkflowCommandStorageException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedLabWorkflowCommandException =
                    new LockedLabWorkflowCommandException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedLabWorkflowCommandException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedLabWorkflowCommandStorageException =
                    new FailedLabWorkflowCommandStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedLabWorkflowCommandStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsLabWorkflowCommandException =
                    new AlreadyExistsLabWorkflowCommandException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsLabWorkflowCommandException);
            }
            catch (Exception exception)
            {
                var failedLabWorkflowCommandServiceException =
                    new FailedLabWorkflowCommandServiceException(exception);

                throw CreateAndLogServiceException(failedLabWorkflowCommandServiceException);
            }
        }


        private LabWorkflowCommandDependencyException CreateAndLogCriticalDependencyException(
            Xeption innerException)
        {
            var labWorkflowCommandDependencyException =
                new LabWorkflowCommandDependencyException(innerException);

            this.loggingBroker.LogCritical(labWorkflowCommandDependencyException);

            return labWorkflowCommandDependencyException;
        }

        private LabWorkflowCommandDependencyException CreateAndLogDependencyException(
            Xeption innerException)
        {
            var labWorkflowCommandDependencyException =
                new LabWorkflowCommandDependencyException(innerException);

            this.loggingBroker.LogError(labWorkflowCommandDependencyException);

            return labWorkflowCommandDependencyException;
        }

        private LabWorkflowCommandDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var labWorkflowCommandDependencyValidationException =
                new LabWorkflowCommandDependencyValidationException(exception);

            this.loggingBroker.LogError(labWorkflowCommandDependencyValidationException);

            return labWorkflowCommandDependencyValidationException;
        }

        private LabWorkflowCommandValidationException CreateAndLogValidationException(Xeption exception)
        {
            var labWorkflowCommandValidationException =
                new LabWorkflowCommandValidationException(exception);

            this.loggingBroker.LogError(labWorkflowCommandValidationException);

            return labWorkflowCommandValidationException;
        }

        private LabWorkflowCommandServiceException CreateAndLogServiceException(Xeption exception)
        {
            var labWorkflowCommandServiceException =
                new LabWorkflowCommandServiceException(exception);

            this.loggingBroker.LogError(labWorkflowCommandServiceException);

            return labWorkflowCommandServiceException;
        }
    }
}
