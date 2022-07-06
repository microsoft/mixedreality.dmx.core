// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Services.Foundations.ExternalLabs;
using DMX.Core.Api.Services.Foundations.Labs;

namespace DMX.Core.Api.Services.Orchestrations
{
    public partial class LabOrchestrationService : ILabOrchestrationService
    {
        private readonly ILabService labService;
        private readonly IExternalLabService externalLabService;
        private readonly ILoggingBroker loggingBroker;

        public LabOrchestrationService(
            ILabService labService,
            IExternalLabService externalLabService,
            ILoggingBroker loggingBroker)
        {
            this.labService = labService;
            this.externalLabService = externalLabService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<Lab>> RetrieveAllLabsAsync() =>
        TryCatch(async () =>
        {
            List<Lab> existingLabs = this.labService.RetrieveAllLabsWithDevices().ToList();
            List<Lab> externalLabs = await this.externalLabService.RetrieveAllExternalLabsAsync();

            List<Lab> onlineLabs = existingLabs.IntersectBy(externalLabs.Select(externalLab =>
                externalLab.ExternalId),
                    (existingLab) =>
                        existingLab.ExternalId)
                            .ToList();

            onlineLabs.ForEach(lab => lab.Status = LabStatus.Available);

            List<Lab> offlineLabs = existingLabs.ExceptBy(externalLabs.Select(externalLab =>
                externalLab.ExternalId),
                    (existingLab) =>
                        existingLab.ExternalId)
                            .ToList();

            offlineLabs.ForEach(lab => lab.Status = LabStatus.Offline);

            List<Lab> unregisteredLabs = externalLabs.ExceptBy(existingLabs.Select(existingLab =>
                existingLab.ExternalId),
                    (externalLab) =>
                        externalLab.ExternalId)
                            .ToList();

            unregisteredLabs.ForEach(lab => lab.Status = LabStatus.Unregistered);

            return onlineLabs.Union(offlineLabs).Union(unregisteredLabs).ToList();
        });

        public static bool LabIsOnline(Lab lab, HashSet<string> externalLabIds) =>
            externalLabIds.Contains(lab.ExternalId);

        public static bool LabDeviceIsOnline(LabDevice labDevice, HashSet<Guid> externalLabDeviceIds) =>
            externalLabDeviceIds.Contains(labDevice.Id);
    }
}
