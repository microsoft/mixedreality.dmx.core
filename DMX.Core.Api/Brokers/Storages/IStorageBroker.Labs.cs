// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Lab> InsertLabAsync(Lab lab);
        IQueryable<Lab> SelectAllLabs();
    }
}
