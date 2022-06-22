// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace DMX.Core.Api.Services.Foundations.Labs
{
    public partial class LabService
    {
        private delegate ValueTask<Lab> ReturningLabFunction();

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
            catch (DbUpdateException dbUpdateException)
            {
                var failedLabStorageException = new FailedLabStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedLabStorageException);
            }
        }

        private LabDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var labDependencyException = new LabDependencyException(exception);
            this.loggingBroker.LogError(labDependencyException);

            return labDependencyException;
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
    }
}
