// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using DMX.Core.Api.Brokers.DateTimes;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Storages;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Services.Foundations.LabWorkflowCommands;
using Moq;
using Tynamix.ObjectFiller;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandServiceTests
    {
        private readonly Mock<IStorageBroker> storageBroker;
        private readonly Mock<ILoggingBroker> loggingBroker;
        private readonly Mock<IDateTimeBroker> datetimeBroker;
        private readonly LabWorkflowCommandService labWorkflowCommandService;

        public LabWorkflowCommandServiceTests()
        {
            this.storageBroker = new Mock<IStorageBroker>();
            this.loggingBroker = new Mock<ILoggingBroker>();
            this.datetimeBroker = new Mock<IDateTimeBroker>();

            this.labWorkflowCommandService = new LabWorkflowCommandService(
                this.storageBroker.Object,
                this.loggingBroker.Object,
                this.datetimeBroker.Object);
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static LabWorkflowCommand CreateRandomLabWorkflowCommand(DateTimeOffset dateTimeOffset) =>
            CreateRandomLabWorkflowCommandFiller(dateTimeOffset).Create();

        private static LabWorkflowCommand CreateRandomLabWorkflowCommand() =>
            CreateRandomLabWorkflowCommandFiller(GetRandomDateTimeOffset()).Create();

        private static Filler<LabWorkflowCommand> CreateRandomLabWorkflowCommandFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<LabWorkflowCommand>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset);

            return filler;
        }
    }
}
