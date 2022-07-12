// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;

namespace DMX.Core.Api.Services.Orchestrations
{
    public interface ILabOrchestrationService
    {
        public ValueTask<List<Lab>> RetrieveAllLabsAsync();
        public ValueTask<Lab> AddLabAsync(Lab lab);
    }
}
