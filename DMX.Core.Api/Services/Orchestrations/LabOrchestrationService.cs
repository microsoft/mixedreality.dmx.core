// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

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

            List<Lab> externalOnlineLabs = externalLabs.IntersectBy(existingLabs.Select(existingLab =>
                existingLab.ExternalId),
                    (externalLab) =>
                        externalLab.ExternalId)
                            .ToList();

            UpdateDeviceStatusForOnlineLabs(onlineLabs, externalOnlineLabs);

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

            unregisteredLabs.ForEach(lab =>
                lab.Devices.ForEach(labDevice =>
                    labDevice.Status = LabDeviceStatus.Unregistered));


            return onlineLabs.Union(offlineLabs).Union(unregisteredLabs).ToList();
        });

        public ValueTask<Lab> AddLabAsync(Lab lab)
        {
            throw new System.NotImplementedException();
        }

        public static void UpdateDeviceStatusForOnlineLabs(List<Lab> onlineLabs, List<Lab> externalOnlineLabs)
        {
            onlineLabs.ForEach(onlineLab =>
            {
                Lab externalLab = externalOnlineLabs.Single(externalLab => externalLab.ExternalId == onlineLab.ExternalId);
                onlineLab.Devices = UpdateLabDeviceStatuses(onlineLab.Devices, externalLab.Devices);
            });
        }

        public static List<LabDevice> UpdateLabDeviceStatuses(
            List<LabDevice> onlineLabDevices,
            List<LabDevice> externalOnlineLabDevices)
        {
            List<LabDevice> onlineDevices =
                onlineLabDevices.IntersectBy(externalOnlineLabDevices.Select(externalOnlineLabDevice =>
                    externalOnlineLabDevice.Name),
                        onlineLabDevice =>
                            onlineLabDevice.Name)
                                .ToList();

            onlineLabDevices.ForEach(labDevice => labDevice.Status = LabDeviceStatus.Online);

            List<LabDevice> offlineDevices =
                onlineLabDevices.ExceptBy(externalOnlineLabDevices.Select(externalOnlineLabDevice =>
                    externalOnlineLabDevice.Name),
                        onlineLabDevice =>
                            onlineLabDevice.Name)
                                .ToList();

            offlineDevices.ForEach(labDevice => labDevice.Status = LabDeviceStatus.Offline);

            List<LabDevice> unregisteredDevices =
                externalOnlineLabDevices.ExceptBy(onlineLabDevices.Select(onlineLabDevice =>
                    onlineLabDevice.Name),
                        externalOnlineLabDevice =>
                            externalOnlineLabDevice.Name)
                                .ToList();

            unregisteredDevices.ForEach(labDevice => labDevice.Status = LabDeviceStatus.Unregistered);

            return onlineDevices.Union(offlineDevices).Union(unregisteredDevices).ToList();
        }
    }
}
