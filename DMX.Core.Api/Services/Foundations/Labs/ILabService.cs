// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Labs;

namespace DMX.Core.Api.Services.Foundations.Labs
{
    public interface ILabService
    {
        ValueTask<Lab> AddLabAsync(Lab lab);
    }
}
