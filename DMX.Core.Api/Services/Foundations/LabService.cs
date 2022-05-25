// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.ReverbApis;
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

        public ValueTask<List<Lab>> RetrieveAllLabsAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
