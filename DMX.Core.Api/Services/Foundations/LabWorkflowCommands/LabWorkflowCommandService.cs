// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Brokers.DateTimes;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Storages;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;

namespace DMX.Core.Api.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandService : ILabWorkflowCommandService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public LabWorkflowCommandService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<LabWorkflowCommand> AddLabWorkflowCommandAsync(LabWorkflowCommand labWorkflowCommand) =>
        TryCatch(async () =>
        {
            ValidateLabWorkflowCommandOnAdd(labWorkflowCommand);

            return await this.storageBroker.InsertLabWorkflowCommandAsync(labWorkflowCommand);
        });

        public ValueTask<LabWorkflowCommand> ModifyLabWorkflowCommand(LabWorkflowCommand labWorkflowCommand) =>
        TryCatch(async () =>
        {
            ValidateLabWorkflowCommandOnModify(labWorkflowCommand);

            LabWorkflowCommand maybeLabWorkflowCommand =
                await this.storageBroker.SelectLabWorkflowCommandByIdAsync(labWorkflowCommand.Id);

            ValidateLabWorkflowCommandAgainstStorageLabWorkflowCommand(
                labWorkflowCommand,
                maybeLabWorkflowCommand);

            return await this.storageBroker.UpdateLabWorkflowCommandAsync(labWorkflowCommand);
        });
    }
}
