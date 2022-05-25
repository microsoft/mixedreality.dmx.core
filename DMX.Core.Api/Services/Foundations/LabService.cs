// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.ReverbApis;
using DMX.Core.Api.Models.External.ExternalLabs;
using DMX.Core.Api.Models.Labs;
using Microsoft.Extensions.Logging;

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
                }).ToList();
        }
    }
}
