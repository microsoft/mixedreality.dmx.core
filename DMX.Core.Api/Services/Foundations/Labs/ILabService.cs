﻿// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;

namespace DMX.Core.Api.Services.Foundations.Labs
{
    public interface ILabService
    {
        ValueTask<Lab> AddLabAsync(Lab lab);
        IQueryable<Lab> RetrieveAllLabsWithDevices();
        ValueTask<Lab> RetrieveLabByIdAsync(Guid labId);
        ValueTask<Lab> RemoveLabByIdAsync(Guid labId);
    }
}
