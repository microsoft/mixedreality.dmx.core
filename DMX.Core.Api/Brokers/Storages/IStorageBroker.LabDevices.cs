// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;

namespace DMX.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<LabDevice> SelectLabDeviceByIdAsync(Guid labDeviceId);
    }
}
