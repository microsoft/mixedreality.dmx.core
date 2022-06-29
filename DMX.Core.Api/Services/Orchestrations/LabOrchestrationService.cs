// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Services.Foundations.ExternalLabs;
using DMX.Core.Api.Services.Foundations.Labs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            List<Lab> externalLabs = await this.externalLabService.RetrieveAllLabsAsync();
            List<Lab> existingLabs = this.labService.RetrieveAllLabs().ToList();
            existingLabs.ForEach(lab => lab.Status = LabStatus.Offline);

            List<Lab> onlineLabs = existingLabs.Where(
                LabIsOnline(externalLabs)).ToList();

            onlineLabs.ForEach(onlineLab =>
                onlineLab.Status = LabStatus.Available);

            return existingLabs;
        }

        public static Func<Lab, bool> LabIsOnline(List<Lab> externalLabs) =>
            existingLab => externalLabs.Any(IsSameExternalId(existingLab));

        public static Func<Lab, bool> IsSameExternalId(Lab existingLab) =>
            externalLab => externalLab.ExternalId == existingLab.ExternalId;
    }
}
