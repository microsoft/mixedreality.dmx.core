// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflowEvent.Exceptions;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;

namespace DMX.Core.Api.Services.Foundations.LabWorkflowEvents
{
    public partial class LabWorkflowEventService
    {
        private delegate ValueTask<LabWorkflow> ReturningLabWorkflowFunction();

        private async ValueTask<LabWorkflow> TryCatch(ReturningLabWorkflowFunction returningLabWorkflowFunction)
        {
            try
            {
                return await returningLabWorkflowFunction();
            }
            catch(NullLabWorkflowException nullLabWorkflowException)
            {
                throw CreateAndLogValidationException(nullLabWorkflowException);
            }
        }

        private LabWorkflowEventValidationException CreateAndLogValidationException(NullLabWorkflowException nullLabWorkflowException)
        {
            var labWorkflowEventValidationException = new LabWorkflowEventValidationException(nullLabWorkflowException);
            this.loggingBroker.LogError(labWorkflowEventValidationException);

            return labWorkflowEventValidationException;
        }
    }
}
