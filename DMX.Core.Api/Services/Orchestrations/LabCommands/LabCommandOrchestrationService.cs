// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Services.Foundations.LabCommands;

namespace DMX.Core.Api.Services.Orchestrations.LabCommands
{
    public partial class LabCommandOrchestrationService : ILabCommandOrchestrationService
    {
        private readonly ILabCommandService labCommandService;
        private readonly ILoggingBroker loggingBroker;

        public LabCommandOrchestrationService(
            ILabCommandService labCommandService,
            ILoggingBroker loggingBroker)
        {
            this.labCommandService = labCommandService;
            this.loggingBroker = loggingBroker;
        }
        public ValueTask<LabCommand> AddLabCommandAsync(LabCommand labCommand)
        {
            throw new System.NotImplementedException();
        }
    }
}
