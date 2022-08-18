// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;

namespace DMX.Core.Api.Tests.Acceptance.Brokers
{
    public partial class DmxCoreApiBroker
    {
        private const string LabCommandApiRelativeUrl = "api/labcommands";

        public async ValueTask<LabCommand> PostLabCommandAsync(LabCommand labCommand)
        {
            return await this.apiFactoryClient.PostContentAsync<LabCommand>(
                relativeUrl: $"{LabCommandApiRelativeUrl}",
                content: labCommand);
        }

        public async ValueTask<LabCommand> GetLabCommandByIdAsync(Guid labCommandId)
        {
            return await this.apiFactoryClient.GetContentAsync<LabCommand>(
                relativeUrl: $"{LabCommandApiRelativeUrl}/{labCommandId}");
        }
    }
}
