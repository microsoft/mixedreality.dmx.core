// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace DMX.Core.Api.Services.Foundations.LabCommands
{
    public partial class LabCommandService : ILabCommandService
    {
        private delegate ValueTask<LabCommand> ReturningLabCommandFunction();

        private async ValueTask<LabCommand> TryCatch(ReturningLabCommandFunction returningLabCommandFunction)
        {
            try
            {
                return await returningLabCommandFunction();
            }
            catch (NullLabCommandException nullLabCommandException)
            {
                throw CreateAndLogValidationException(nullLabCommandException);
            }
            catch (InvalidLabCommandException invalidLabCommandException)
            {
                throw CreateAndLogValidationException(invalidLabCommandException);
            }
            catch (NotFoundLabCommandException notFoundLabCommandException)
            {
                throw CreateAndLogValidationException(notFoundLabCommandException);
            }
            catch (SqlException sqlException)
            {
                var failedLabCommandStorageException =
                    new FailedLabCommandStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedLabCommandStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsLabException =
                    new AlreadyExistsLabCommandException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsLabException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedLabCommandStorageException =
                    new FailedLabCommandStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedLabCommandStorageException);
            }
            catch (Exception exception)
            {
                var failedLabCommandServiceException = new FailedLabCommandServiceException(exception);

                throw CreateAndLogServiceException(failedLabCommandServiceException);
            }
        }

        private LabCommandValidationException CreateAndLogValidationException(Xeption exception)
        {
            var labCommandValidationException =
                new LabCommandValidationException(exception);

            this.loggingBroker.LogError(labCommandValidationException);

            return labCommandValidationException;
        }

        private LabCommandDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var labCommandDependencyException =
                new LabCommandDependencyException(exception);

            this.loggingBroker.LogCritical(labCommandDependencyException);

            return labCommandDependencyException;
        }

        private LabCommandDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var labCommandDependencyValidationException =
                new LabCommandDependencyValidationException(exception);

            this.loggingBroker.LogError(labCommandDependencyValidationException);

            return labCommandDependencyValidationException;
        }

        private LabCommandDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var labCommandDependencyException =
                new LabCommandDependencyException(exception);

            this.loggingBroker.LogError(labCommandDependencyException);

            return labCommandDependencyException;
        }

        private LabCommandServiceException CreateAndLogServiceException(Xeption exception)
        {
            var labCommandServiceException =
                new LabCommandServiceException(exception);

            this.loggingBroker.LogError(labCommandServiceException);

            return labCommandServiceException;
        }
    }
}
