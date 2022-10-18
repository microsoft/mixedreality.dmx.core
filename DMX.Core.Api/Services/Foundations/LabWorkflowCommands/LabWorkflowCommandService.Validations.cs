// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands.Exceptions;

namespace DMX.Core.Api.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandService
    {
        private static void ValidateLabWorkflowCommandIsNotNull(LabWorkflowCommand labWorkflowCommand)
        {
            if (labWorkflowCommand is null)
            {
                throw new NullLabWorkflowCommandException();
            }
        }
    }
}
