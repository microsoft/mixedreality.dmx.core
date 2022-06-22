// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.ExternalLabs;

namespace DMX.Core.Api.Brokers.LabApis
{
    public partial interface IExternalLabApiBroker
    {
        ValueTask<ExternalLabCollection> GetAvailableLabsAsync(
            ExternalLabServiceInformation externalLabServiceInformation);
    }
}
