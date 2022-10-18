// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;

namespace DMX.Core.Api.Services.Foundations.LabWorkflowEvents
{
    public partial class LabWorkflowEventService
    {
        private void ValidateIfNull(LabWorkflow labWorkflow)
        {
            if (labWorkflow is null)
            {
                throw new NullLabWorkflowException();
            }
        }
    }
}
