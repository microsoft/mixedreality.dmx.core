﻿// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;

namespace DMX.Core.Api.Services.Orchestrations.LabCommands
{
    public interface ILabCommandOrchestrationService
    {
        ValueTask<LabCommand> AddLabCommandAsync(LabCommand labCommand);
    }
}
