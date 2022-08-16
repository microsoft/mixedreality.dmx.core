// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Tests.Acceptance.Brokers;
using Tynamix.ObjectFiller;
using Xunit;

namespace DMX.Core.Api.Tests.Acceptance.APIs.LabCommands
{
    [Collection(nameof(ApiTestCollection))]
    public partial class LabCommandApiTests
    {
        private readonly DmxCoreApiBroker dmxCoreApiBroker;

        public LabCommandApiTests(DmxCoreApiBroker dmxCoreApiBroker)
        {
            this.dmxCoreApiBroker = dmxCoreApiBroker;
        }

        private static LabCommand CreateRandomLabCommand() =>
            CreateRandomLabCommandFiller().Create();

        private static Filler<LabCommand> CreateRandomLabCommandFiller()
        {
            var filler = new Filler<LabCommand>();
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid id = Guid.NewGuid();

            filler.Setup()
                .OnProperty(labCommand => labCommand.Id).Use(id)
                .OnProperty(labCommand => labCommand.LabId).Use(id)
                .OnProperty(labCommand => labCommand.CreatedDate).Use(now)
                .OnProperty(labCommand => labCommand.UpdatedDate).Use(now);

            return filler;
        }
    }
}
