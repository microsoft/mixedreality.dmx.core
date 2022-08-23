// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using DMX.Core.Api.Models.Foundations.Labs.Exceptions;
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
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private ILabCommandOrchestrationService labCommandOrchestrationService;

        public LabCommandOrchestrationServiceTests()
        {
            this.labCommandServiceMock = new Mock<ILabCommandService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.labCommandOrchestrationService = new LabCommandOrchestrationService(
                labCommandService: this.labCommandServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        public static TheoryData<Xeption> LabCommandDependencyValidationExceptions()
        {
            string randomErrorMessage = GetRandomString();
            var innerException = new Xeption(randomErrorMessage);

            return new TheoryData<Xeption>
            {
                new LabCommandValidationException(innerException),
                new LabCommandDependencyValidationException(innerException),
            };
        }

        public static TheoryData<Xeption> LabCommandDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            var innerException = new Xeption(randomMessage);

            return new TheoryData<Xeption>
            {
                new LabCommandDependencyException(innerException),
                new LabCommandServiceException(innerException)
            };
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static LabCommand CreateRandomLabCommand() =>
            CreateLabCommandFiller(GetRandomDateTimeOffset()).Create();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static LabCommand CreateRandomLabCommand(DateTimeOffset date) =>
            CreateLabCommandFiller(date).Create();

        private static Filler<LabCommand> CreateLabCommandFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<LabCommand>();

            filler.Setup()
                .OnType<DateTimeOffset>()
                    .Use(dateTimeOffset);

            return filler;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);
    }
}
