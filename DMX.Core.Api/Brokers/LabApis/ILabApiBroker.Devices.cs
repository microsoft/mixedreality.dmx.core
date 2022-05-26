// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.External.ExternalLabs;

namespace DMX.Core.Api.Brokers.LabApis
{
    public partial interface ILabApiBroker
    {
        ValueTask<ExternalLabsCollection> GetAvailableDevicesAsync(
            ExternalLabsServiceInformation externalLabsServiceInformation);
    }
}
