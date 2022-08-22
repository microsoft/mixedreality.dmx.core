// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Orchestrations.LabCommands.Exceptions;

namespace DMX.Core.Api.Services.Orchestrations.LabCommands
{
    public partial class LabCommandOrchestrationService
    {
        private static void ValidateLabCommand(LabCommand labCommand)
        {
            if (labCommand is null)
            {
                throw new NullLabCommandOrchestrationException();
            }
        }
    }
}
