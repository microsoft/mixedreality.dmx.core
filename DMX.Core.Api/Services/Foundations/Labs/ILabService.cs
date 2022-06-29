// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;

namespace DMX.Core.Api.Services.Foundations.Labs
{
    public interface ILabService
    {
        ValueTask<Lab> AddLabAsync(Lab lab);
        IQueryable<Lab> RetrieveAllLabs();
    }
}
