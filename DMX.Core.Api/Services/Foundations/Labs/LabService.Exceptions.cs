// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Foundations.Labs.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace DMX.Core.Api.Services.Foundations.Labs
{
    public partial class LabService
    {
        private delegate ValueTask<Lab> ReturningLabFunction();

        private delegate IQueryable<Lab> ReturningLabsFunction();

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
            catch (SqlException sqlException)
            {
                var failedLabStorageException = new FailedLabStorageException(sqlException);

                throw CreateAndLogCriticalLabDependencyException(failedLabStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsLabException = new AlreadyExistsLabException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsLabException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedLabException = new LockedLabException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedLabException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedLabStorageException = new FailedLabStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedLabStorageException);
            }
            catch (NotFoundLabException notFoundLabException)
            {
                throw CreateAndLogValidationException(notFoundLabException);
            }
            catch (Exception exception)
            {
                var failedLabServiceException = new FailedLabServiceException(exception);

                throw CreateAndLogServiceException(failedLabServiceException);
            }
        }

        private IQueryable<Lab> TryCatch(ReturningLabsFunction returningLabsFunction)
        {
            try
            {
                return returningLabsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedLabStorageException = new FailedLabStorageException(sqlException);

                throw CreateAndLogCriticalLabDependencyException(failedLabStorageException);
            }
            catch (Exception exception)
            {
                var failedLabServiceException = new FailedLabServiceException(exception);

                throw CreateAndLogServiceException(failedLabServiceException);
            }
        }

        private LabValidationException CreateAndLogValidationException(Xeption exception)
        {
            var labValidationException = new LabValidationException(exception);
            this.loggingBroker.LogError(exception: labValidationException);

            return labValidationException;
        }

        private LabDependencyException CreateAndLogCriticalLabDependencyException(Xeption exception)
        {
            var labDependencyException = new LabDependencyException(exception);
            this.loggingBroker.LogCritical(exception: labDependencyException);

            return labDependencyException;
        }

        private LabDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var labDependencyValidationException = new LabDependencyValidationException(exception);
            this.loggingBroker.LogError(exception: labDependencyValidationException);

            return labDependencyValidationException;
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
