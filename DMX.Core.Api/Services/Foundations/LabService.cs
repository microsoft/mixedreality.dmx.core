// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using DMX.Core.Api.Brokers.ReverbApis;
using DMX.Core.Api.Models.Labs;
using Microsoft.Extensions.Logging;

namespace DMX.Core.Api.Services.Foundations
{
    public class LabService : ILabService
    {
        private readonly IReverbApiBroker reverbApiBroker;
        private readonly ILoggingBuilder loggingBuilder;

        public LabService(
            IReverbApiBroker reverbApiBroker,
            ILoggingBuilder loggingBuilder)
        {
            this.reverbApiBroker = reverbApiBroker;
            this.loggingBuilder = loggingBuilder;
        }

        public ValueTask<List<Lab>> RetrieveAllLabsAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
