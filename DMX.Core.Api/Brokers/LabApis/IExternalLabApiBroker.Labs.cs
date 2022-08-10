// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.ExternalLabs;

namespace DMX.Core.Api.Brokers.LabApis
{
    public partial interface IExternalLabApiBroker
    {
        ValueTask<ExternalLabCollection> GetAllLabsAsync();
        ValueTask<ExternalLabCollection> GetAvailableLabsAsync();
    }
}
