// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Storages;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using DMX.Core.Api.Services.Foundations.LabWorkflows;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflows
{
    public partial class LabWorkflowTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ILabWorkflowService labWorkflowService;

        public LabWorkflowTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.labWorkflowService = new LabWorkflowService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static LabWorkflow CreateRandomLabWorkflow() =>
            CreateLabWorkflowFiller(GetRandomDateTimeOffset()).Create();

        private static T GetInvalidEnum<T>()
        {
            int randomNumber = GetLocalRandomNumber();

            while (Enum.IsDefined(typeof(T), randomNumber))
            {
                randomNumber = GetLocalRandomNumber();
            }

            return (T)(object)randomNumber;

            static int GetLocalRandomNumber() =>
                new IntRange(min: int.MinValue, max: int.MaxValue).GetValue();
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static SqlException GetSqlException() =>
           (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Filler<LabWorkflow> CreateLabWorkflowFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<LabWorkflow>();

            filler.Setup()
                .OnType<DateTimeOffset>()
                    .Use(dateTimeOffset);

            return filler;
        }
    }
}
