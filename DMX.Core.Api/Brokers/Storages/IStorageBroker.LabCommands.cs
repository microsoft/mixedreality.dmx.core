// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<LabCommand> InsertLabCommandAsync(LabCommand labCommand);
        ValueTask<LabCommand> SelectLabCommandByIdAsync(Guid labCommandId);
        ValueTask<LabCommand> UpdateLabCommandAsync(LabCommand command);
        ValueTask<LabCommand> DeleteLabCommandByIdAsync(LabCommand labCommand);
    }
}