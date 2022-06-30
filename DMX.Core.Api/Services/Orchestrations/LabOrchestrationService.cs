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
            existingLabs.ForEach(lab => lab.Status = LabStatus.Offline);

            List<LabDevice> existingLabDevices =
                existingLabs.SelectMany(lab => lab.Devices,
                    (lab, labDevice) => labDevice).ToList();

            existingLabDevices.ForEach(labDevice =>
                labDevice.Status = LabDeviceStatus.Offline);

            HashSet<string> externalLabsIds =
                externalLabs.Select(lab =>
                    lab.ExternalId)
                        .ToHashSet();

            List<Lab> onlineLabs = existingLabs.Where((lab) =>
                LabIsOnline(lab, externalLabsIds)).ToList();

            onlineLabs.ForEach(onlineLab =>
                onlineLab.Status = LabStatus.Available);

            HashSet<Guid> externalLabDevicesIds =
                externalLabs.SelectMany(lab => lab.Devices,
                    (lab, labDevice) => labDevice.Id)
                        .ToHashSet();

            List<LabDevice> onlineLabDevices = existingLabDevices.Where(labDevice =>
                LabDeviceIsOnline(labDevice, externalLabDevicesIds))
                    .ToList();

            onlineLabDevices.ForEach(onlineLabDevice =>
                onlineLabDevice.Status = LabDeviceStatus.Online);

            return existingLabs;
        });

        public static bool LabIsOnline(Lab lab, HashSet<string> externalLabIds) =>
            externalLabIds.Contains(lab.ExternalId);

        public static bool LabDeviceIsOnline(LabDevice labDevice, HashSet<Guid> externalLabDeviceIds) =>
            externalLabDeviceIds.Contains(labDevice.Id);
    }
}
