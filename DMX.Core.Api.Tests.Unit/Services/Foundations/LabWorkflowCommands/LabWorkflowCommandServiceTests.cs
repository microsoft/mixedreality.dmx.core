// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using DMX.Core.Api.Brokers.DateTimes;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Storages;
using DMX.Core.Api.Models.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Services.Foundations.LabWorkflowCommands;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflowCommands
{
    public partial class LabWorkflowCommandServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> datetimeBrokerMock;
        private readonly LabWorkflowCommandService labWorkflowCommandService;

        public LabWorkflowCommandServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.datetimeBrokerMock = new Mock<IDateTimeBroker>();

            this.labWorkflowCommandService = new LabWorkflowCommandService(
                this.storageBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.datetimeBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            GetRandomNumber() * -1;

        private static T GetInvalidEnum<T>()
        {
            int randomNumber = GetRandomNumber();

            while (Enum.IsDefined(typeof(T), randomNumber))
            {
                randomNumber = GetRandomNumber();
            }

            return (T)(object)randomNumber;
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
