// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommandEvents;

namespace DMX.Core.Api.Services.Foundations.LabCommandEvents
{
    public partial interface ILabCommandEventService
    {
        ValueTask<LabCommandEvent> AddLabCommandEventAsync(LabCommandEvent labCommandEvent);
    }
}
