// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using Microsoft.EntityFrameworkCore;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Lab> Labs { get; set; }

        public async ValueTask<Lab> InsertLabAsync(Lab lab) =>
            await InsertAsync(lab);

        public IQueryable<Lab> SelectAllLabsWithDevices() =>
            SelectAll<Lab>().Include(lab => lab.Devices);

        public async ValueTask<Lab> SelectLabByIdAsync(Guid labId) =>
            await FindAsync<Lab>(labId);

        public async ValueTask<Lab> DeleteLabAsync(Lab lab) =>
            await DeleteAsync(lab);
    }
}
