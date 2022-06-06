// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Externals.ExternalLabs;

namespace DMX.Core.Api.Brokers.LabApis
{
    public partial interface ILabApiBroker
    {
        ValueTask<ExternalLabCollection> GetAvailableLabsAsync(
            ExternalLabServiceInformation externalLabServiceInformation);
    }
}
