// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabCommands;
using System.Threading.Tasks;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<LabCommand> UpdateLabCommandAsync(LabCommand command);
        ValueTask<LabCommand> InsertLabCommandAsync(LabCommand labCommand);
    }
}
