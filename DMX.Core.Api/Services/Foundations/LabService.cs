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
    public class LabService : ILabService
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

        public async ValueTask<List<Lab>> RetrieveAllLabsAsync()
        {
            try
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
            }

            catch (Exception exception) when (exception
                is HttpResponseUrlNotFoundException
                or HttpResponseUnauthorizedException
                or HttpResponseForbiddenException)
            {
                var failedLabDependencyException = new FailedLabDependencyException(exception);
                var labDependencyException = new LabDependencyException(failedLabDependencyException);
                this.loggingBroker.LogCritical(labDependencyException);

                throw labDependencyException;
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedLabDependencyException = new FailedLabDependencyException(httpResponseException);
                var labDependencyException = new LabDependencyException(failedLabDependencyException);
                this.loggingBroker.LogError(labDependencyException);

                throw labDependencyException;
            }
            catch (Exception exception)
            {
                var failedLabServiceException = new FailedLabServiceException(exception);
                var labServiceException = new LabServiceException(failedLabServiceException);
                this.loggingBroker.LogError(labServiceException);

                throw labServiceException;
            }
        }

        private List<LabDevice> RetrieveDevices(ExternalLab externalLab)
        {
            var devices = new List<LabDevice>();

            if (externalLab.Properties.ContainsKey(@"Host\\isconnected"))
            {
                devices.Add(new LabDevice
                {
                    Category = LabDeviceCategory.Host,
                    Type = LabDeviceType.PC,

                    Status = bool.Parse(externalLab.Properties[@"Host\\isconnected"])
                        ? LabDeviceStatus.Online
                        : LabDeviceStatus.Offline,
                });
            }

            if (externalLab.Properties.ContainsKey(@"Phone\\isconnected"))
            {
                devices.Add(new LabDevice
                {
                    Name = externalLab.Properties[@"Phone\\name"],
                    Category = LabDeviceCategory.Attachment,
                    Type = LabDeviceType.Phone,

                    Status = bool.Parse(externalLab.Properties[@"Phone\\isconnected"])
                        ? LabDeviceStatus.Online
                        : LabDeviceStatus.Offline,
                });

            }

            if (externalLab.Properties.ContainsKey(@"HMD\\isconnected"))
            {
                devices.Add(new LabDevice
                {
                    Name = externalLab.Properties[@"HMD\\name"],
                    Category = LabDeviceCategory.Attachment,
                    Type = LabDeviceType.HeadMountedDisplay,

                    Status = bool.Parse(externalLab.Properties[@"HMD\\isconnected"])
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
