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
            List<Lab> existingLabs = this.labService.RetrieveAllLabs().ToList();
            List<Lab> externalLabs = await this.externalLabService.RetrieveAllExternalLabsAsync();
            existingLabs.ForEach(lab => lab.Status = LabStatus.Offline);

            List<Lab> onlineLabs = existingLabs.Where(
                LabIsOnline(externalLabs)).ToList();

            onlineLabs.ForEach(onlineLab =>
                onlineLab.Status = LabStatus.Available);

            return existingLabs;
        });

        public static Func<Lab, bool> LabIsOnline(List<Lab> externalLabs) =>
            existingLab => externalLabs.Any(IsSameExternalId(existingLab));

        public static Func<Lab, bool> IsSameExternalId(Lab existingLab) =>
            externalLab => externalLab.ExternalId == existingLab.ExternalId;
    }
}
