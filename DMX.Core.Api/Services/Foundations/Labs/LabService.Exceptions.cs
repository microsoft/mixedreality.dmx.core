// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
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
