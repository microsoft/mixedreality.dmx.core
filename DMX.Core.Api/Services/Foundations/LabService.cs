// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.ReverbApis;
using DMX.Core.Api.Models.External.ExternalLabs;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using RESTFulSense.Exceptions;

namespace DMX.Core.Api.Services.Foundations
{
    public partial class LabService : ILabService
    {
        private readonly IReverbApiBroker reverbApiBroker;
        private readonly ILoggingBroker loggingBroker;

        public LabService(
            IReverbApiBroker reverbApiBroker,
            ILoggingBroker loggingBroker)
        {
            this.reverbApiBroker = reverbApiBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<Lab>> RetrieveAllLabsAsync() =>
        TryCatch(async () =>
        {
            var externalLabsServiceInformation = new ExternalLabsServiceInformation
            {
                ServiceId = "Bondi-HW-Lab",
                ServiceType = "AzureIotHub"
            };

            ExternalLabsCollection externalLabsCollection =
                await this.reverbApiBroker.GetAvailableDevicesAsync(externalLabsServiceInformation);

            List<ExternalLab> externalLabs = externalLabsCollection.Devices.ToList();

            return externalLabs.Select(externalLab =>
                new Lab
                {
                    ExternalId = externalLab.Id,
                    Name = externalLab.Name,
                    Status = this.RetrieveLabStatus(externalLab),
                    Devices = this.RetrieveDevices(externalLab)
                }).ToList();
        });

        private List<LabDevice> RetrieveDevices(ExternalLab externalLab)
        {
            var devices = new List<LabDevice>();

            if (externalLab.Properties.ContainsKey(@"Host\isconnected"))
            {
                devices.Add(new LabDevice
                {
                    Category = LabDeviceCategory.Host,
                    Type = LabDeviceType.PC,

                    Status = bool.Parse(externalLab.Properties[@"Host\isconnected"])
                        ? LabDeviceStatus.Online
                        : LabDeviceStatus.Offline,
                });
            }

            if (externalLab.Properties.ContainsKey(@"Phone\isconnected"))
            {
                devices.Add(new LabDevice
                {
                    Name = externalLab.Properties[@"Phone\name"],
                    Category = LabDeviceCategory.Attachment,
                    Type = LabDeviceType.Phone,

                    Status = bool.Parse(externalLab.Properties[@"Phone\isconnected"])
                        ? LabDeviceStatus.Online
                        : LabDeviceStatus.Offline,
                });

            }

            if (externalLab.Properties.ContainsKey(@"HMD\isconnected"))
            {
                devices.Add(new LabDevice
                {
                    Name = externalLab.Properties[@"HMD\name"],
                    Category = LabDeviceCategory.Attachment,
                    Type = LabDeviceType.HeadMountedDisplay,

                    Status = bool.Parse(externalLab.Properties[@"HMD\isconnected"])
                        ? LabDeviceStatus.Online
                        : LabDeviceStatus.Offline,
                });

            }

            return devices;
        }

        private LabStatus RetrieveLabStatus(ExternalLab lab)
        {
            return (lab.IsConnected, lab.IsReserved) switch
            {
                (false, _) => LabStatus.Offline,
                (true, false) => LabStatus.Available,
                _ => LabStatus.Reserved,
            };
        }
    }
}
