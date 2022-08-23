// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommandEvents;
using DMX.Core.Api.Models.Foundations.LabCommands;

namespace DMX.Core.Api.Services.Foundations.LabCommandEvents
{
    public partial interface ILabCommandEventService
    {
        ValueTask<LabCommand> AddLabCommandEventAsync(LabCommand labCommand);
    }
}
