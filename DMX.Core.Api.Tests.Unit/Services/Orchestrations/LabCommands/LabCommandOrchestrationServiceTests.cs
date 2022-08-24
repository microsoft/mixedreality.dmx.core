// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using DMX.Core.Api.Services.Foundations.LabCommandEvents;
using DMX.Core.Api.Services.Foundations.LabCommands;
using DMX.Core.Api.Services.Orchestrations.LabCommands;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations.LabCommands
{
    public partial class LabCommandOrchestrationServiceTests
    {
        private readonly Mock<ILabCommandService> labCommandServiceMock;
        private readonly Mock<ILabCommandEventService> labCommandEventServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private ILabCommandOrchestrationService labCommandOrchestrationService;

        public LabCommandOrchestrationServiceTests()
        {
            this.labCommandServiceMock = new Mock<ILabCommandService>();
            this.labCommandEventServiceMock = new Mock<ILabCommandEventService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.labCommandOrchestrationService = new LabCommandOrchestrationService(
                labCommandService: this.labCommandServiceMock.Object,
                labCommandEventService: this.labCommandEventServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        public static TheoryData<Xeption> LabCommandDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            var innerException = new Xeption(randomMessage);

            return new TheoryData<Xeption>
            {
                new LabCommandValidationException(innerException),
                new LabCommandDependencyValidationException(innerException),
                new LabCommandEventValidationException(innerException),
                new LabCommandEventDependencyValidationException(innerException)
            };
        }

        public static TheoryData<Xeption> LabCommandDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            var innerException = new Xeption(randomMessage);

            return new TheoryData<Xeption>
            {
                new LabCommandDependencyException(innerException),
                new LabCommandServiceException(innerException),
                new LabCommandEventDependencyException(innerException),
                new LabCommandEventServiceException(innerException)
            };
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static LabCommand CreateRandomLabCommand() =>
            CreateLabCommandFiller(GetRandomDateTimeOffset()).Create();

        private static LabCommand CreateRandomLabCommand(DateTimeOffset date) =>
            CreateLabCommandFiller(date).Create();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static Filler<LabCommand> CreateLabCommandFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<LabCommand>();

            filler.Setup()
                .OnType<DateTimeOffset>()
                    .Use(dateTimeOffset);

            return filler;
        }
    }
}
