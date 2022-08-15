// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;

namespace DMX.Core.Api.Services.Foundations.LabCommands
{
    public partial interface ILabCommandService
    {
        ValueTask<LabCommand> AddLabCommandAsync(LabCommand labCommand);
    }
}
