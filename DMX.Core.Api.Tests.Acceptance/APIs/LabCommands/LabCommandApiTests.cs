// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Tests.Acceptance.Brokers;
using Microsoft.Extensions.Hosting;
using Tynamix.ObjectFiller;
using Xunit;

namespace DMX.Core.Api.Tests.Acceptance.APIs.LabCommands
{
    [Collection(nameof(ApiTestCollection))]
    public partial class LabCommandApiTests
    {
        private readonly DmxCoreApiBroker dmxCoreApiBroker;

        public LabCommandApiTests(DmxCoreApiBroker dmxCoreApiBroker) =>
            this.dmxCoreApiBroker = dmxCoreApiBroker;

        private async ValueTask<LabCommand> PostRandomLabCommandAsync()
        {
            LabCommand randomLabCommand = CreateRandomLabCommand();
            await this.dmxCoreApiBroker.PostLabCommandAsync(randomLabCommand);

            return randomLabCommand;
        }

        private static LabCommand UpdateRandomLabCommand(LabCommand inputLabCommand)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<LabCommand>();

            filler.Setup()
                .OnProperty(labCommand => labCommand.Id).Use(inputLabCommand.Id)
                .OnProperty(labCommand => labCommand.CreatedDate).Use(inputLabCommand.CreatedDate)
                .OnProperty(labCommand => labCommand.UpdatedDate).Use(now);
                //.OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static LabCommand CreateRandomLabCommand() =>
            CreateRandomLabCommandFiller().Create();

        private static Filler<LabCommand> CreateRandomLabCommandFiller()
        {
            var filler = new Filler<LabCommand>();
            DateTimeOffset now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now);

            return filler;
        }
    }
}
