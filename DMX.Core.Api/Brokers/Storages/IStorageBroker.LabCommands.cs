// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<LabCommand> InsertLabCommandAsync(LabCommand labCommand);
        IQueryable<LabCommand> SelectAllLabCommands();
        ValueTask<LabCommand> SelectLabCommandByIdAsync(Guid labCommandId);
        ValueTask<LabCommand> UpdateLabCommandAsync(LabCommand command);
        ValueTask<LabCommand> DeleteLabCommandAsync(LabCommand labCommand);
    }
}