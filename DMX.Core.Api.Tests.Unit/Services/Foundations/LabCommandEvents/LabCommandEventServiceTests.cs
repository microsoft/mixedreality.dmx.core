// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Queues;
using DMX.Core.Api.Models.Foundations.LabCommandEvents;
using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Services.Foundations.LabCommandEvents;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Azure.ServiceBus;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using AzureMessagingCommunicationException = Microsoft.ServiceBus.Messaging.MessagingCommunicationException;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabCommandEvents
{
    public partial class LabCommandEventServiceTests
    {
        private readonly Mock<IQueueBroker> queueBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICompareLogic compareLogic;

        private readonly ILabCommandEventService labCommandEventService;

        public LabCommandEventServiceTests()
        {
            this.queueBrokerMock = new Mock<IQueueBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.labCommandEventService = new LabCommandEventService(
                queueBroker: this.queueBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object
            );
        }

        public static TheoryData MessageQueueExceptions()
        {
            string message = GetRandomString();

            return new TheoryData<Exception>
            {
                new MessagingEntityNotFoundException(message),
                new MessagingEntityDisabledException(message),
                new UnauthorizedAccessException()
            };
        }

        public static TheoryData MessageQueueDependencyExceptions()
        {
            string message = GetRandomString();

            return new TheoryData<Exception>
            {
                new InvalidOperationException(),
                new AzureMessagingCommunicationException(communicationPath: message),
                new ServerBusyException(message),
            };
        }

        private Expression<Func<Exception, bool>> SameExceptionAs(Xeption exception) =>
            actualException => actualException.SameExceptionAs(exception);

        private static LabCommand CreateRandomLabCommand() =>
            CreateLabCommandFiller().Create();

        private static Filler<LabCommand> CreateLabCommandFiller()
        {
            var filler = new Filler<LabCommand>();

            filler.Setup()
                .OnType<DateTimeOffset>()
                    .Use(GetRandomDateTimeOffset());

            return filler;
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(TimeZoneInfo.Utc).GetValue();

        private Expression<Func<Message, bool>> SameMessageAs(Message expectedMessage)
        {
            return actualMessage =>
                this.compareLogic.Compare(
                    expectedMessage, actualMessage).AreEqual;
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();
    }
}
