// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Orchestrations.LabWorkflows;
using Xeptions;

namespace DMX.Core.Api.Services.Orchestrations.LabWorkflows
{
    public partial class LabWorkflowOrchestrationService : ILabWorkflowOrchestrationService
    {
        private delegate ValueTask<LabWorkflow> ReturningLabWorkflowFunction();

        private async ValueTask<LabWorkflow> TryCatch(ReturningLabWorkflowFunction returningLabWorkflowFunction)
        {
            try
            {
                return await returningLabWorkflowFunction();
            }
            catch (InvalidLabWorkflowOrchestrationException invalidLabWorkflowOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidLabWorkflowOrchestrationException);
            }
        }

        private LabWorkflowOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var labWorkflowOrchestrationValidationException =
                new LabWorkflowOrchestrationValidationException(exception);

            this.loggingBroker.LogError(labWorkflowOrchestrationValidationException);

            return labWorkflowOrchestrationValidationException;
        }
    }
}
