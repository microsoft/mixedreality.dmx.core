﻿// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Queues;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Services.Foundations.LabWorkflowEvents;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Azure.ServiceBus;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using AzureMessagingCommunicationException = Microsoft.ServiceBus.Messaging.MessagingCommunicationException;


namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflowEvents
{
    public partial class LabWorkflowEventServiceTests
    {
        private readonly Mock<IQueueBroker> queueBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly LabWorkflowEventService labWorkflowEventService;

        public LabWorkflowEventServiceTests()
        {
            this.queueBrokerMock = new Mock<IQueueBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.labWorkflowEventService = new LabWorkflowEventService(
                this.queueBrokerMock.Object,
                this.loggingBrokerMock.Object);
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

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private Expression<Func<Message, bool>> SameMessageAs(Message expectedMessage)
        {
            return actualMessage =>
                this.compareLogic.Compare(
                    expectedMessage, actualMessage).AreEqual;
        }

        private Expression<Func<Exception, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(TimeZoneInfo.Utc).GetValue();

        private static LabWorkflow CreateRandomLabWorkflow() =>
            CreateLabWorkflowFiller().Create();

        private static Filler<LabWorkflow> CreateLabWorkflowFiller()
        {
            var filler = new Filler<LabWorkflow>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(GetRandomDateTimeOffset());

            return filler;
        }

    }
}
