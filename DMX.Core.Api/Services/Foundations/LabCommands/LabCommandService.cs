// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Storages;
using DMX.Core.Api.Models.Foundations.LabCommands;

namespace DMX.Core.Api.Services.Foundations.LabCommands
{
    public partial class LabCommandService : ILabCommandService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public LabCommandService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<LabCommand> RetrieveLabCommandByIdAsync(Guid labCommandId) =>
        TryCatch(async () =>
        {
            ValidateLabCommandId(labCommandId);

            LabCommand maybeLabCommand =
                await this.storageBroker.SelectLabCommandByIdAsync(labCommandId);

            ValidateLabCommandExists(maybeLabCommand, labCommandId);

            return maybeLabCommand;
        });

        public ValueTask<LabCommand> ModifyLabCommandAsync(LabCommand labCommand) =>
            throw new NotImplementedException();
    }
}
