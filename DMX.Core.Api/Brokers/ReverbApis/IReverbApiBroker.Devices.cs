// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.External.ExternalLabs;

namespace DMX.Core.Api.Brokers.ReverbApis
{
    public partial interface IReverbApiBroker
    {
        ValueTask<ExternalLabCollection> GetAvailableDevicesAsync(string reverbServiceId);
    }
}
