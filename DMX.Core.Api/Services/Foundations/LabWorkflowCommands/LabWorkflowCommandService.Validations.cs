// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflows.Exceptions;

namespace DMX.Core.Api.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandService
    {
        public void ValidateLabWorkflowCommandIsNull(LabWorkflowCommand LabWorkflowCommand)
        {
            if (LabWorkflowCommand is null)
            {
                throw new NullLabWorkflowCommandException();
            }
        }
    }
}
