// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Services.Foundations.ExternalLabs;
using DMX.Core.Api.Services.Foundations.Labs;

namespace DMX.Core.Api.Services.Orchestrations
{
    public class LabOrchestrationService : ILabOrchestrationService
    {
        private readonly ILabService labService;
        private readonly IExternalLabService externalLabService;

        public LabOrchestrationService(
            ILabService labService,
            IExternalLabService externalLabService)
        {
            this.labService = labService;
            this.externalLabService = externalLabService;
        }

        public async ValueTask<List<Lab>> RetrieveAllLabsAsync()
        {
            await this.externalLabService.RetrieveAllLabsAsync();
            return this.labService.RetrieveAllLabs().ToList();
        }
    }
}
