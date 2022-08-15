// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using Microsoft.Data.SqlClient;
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
                var failedLabCommandStorageException = new FailedLabCommandStorageException(sqlException);

                throw CreateAndLogDepedendencyException(failedLabCommandStorageException);
            }
            catch (Exception exception)
            {
                var failedLabCommandServiceException = new FailedLabCommandServiceException(exception);

                throw CreateAndLogServiceException(failedLabCommandServiceException);
            }
        }

        private LabCommandValidationException CreateAndLogValidationException(Xeption exception)
        {
            var labCommandValidationException = new LabCommandValidationException(exception);
            this.loggingBroker.LogError(exception: labCommandValidationException);

            return labCommandValidationException;
        }

        private LabCommandDependencyException CreateAndLogDepedendencyException(Xeption exception)
        {
            var labCommandDependencyException = new LabCommandDependencyException(exception);
            this.loggingBroker.LogCritical(labCommandDependencyException);

            return labCommandDependencyException;
        }

        private Exception CreateAndLogServiceException(Xeption exception)
        {
            var LabCommandServiceException = new LabCommandServiceException(exception);
            this.loggingBroker.LogError(exception: LabCommandServiceException);

            return LabCommandServiceException;
        }
    }
}
