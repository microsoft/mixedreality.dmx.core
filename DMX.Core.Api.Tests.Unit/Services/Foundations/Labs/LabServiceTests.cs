// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Storages;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using DMX.Core.Api.Services.Foundations.Labs;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.Labs
{
    public partial class LabServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ILabService labService;

        public LabServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.labService = new LabService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        public static TheoryData LabInvalidProperties()
        {
            return new TheoryData<(Lab, InvalidLabException)>()
            {
                CreateLabExceptionPair(nameof(Lab.Id), Guid.Empty, "Id is required"),
                CreateLabExceptionPair(nameof(Lab.ExternalId), null, "Id is required"),
                CreateLabExceptionPair(nameof(Lab.Name), null, "Text is required"),
                CreateLabExceptionPair(nameof(Lab.Description), null, "Text is required"),
            };
        }

        private static (Lab, InvalidLabException) CreateLabExceptionPair(string propertyName, object value, string message)
        {
            var lab = SetLabProperty(CreateRandomLab(), propertyName, value);
            var invalidLabException = new InvalidLabException();

            invalidLabException.AddData(
                key: propertyName,
                values: message);

            return (lab, invalidLabException);
        }

        private static Lab SetLabProperty(Lab lab, string propertyName, object propertyValue)
        {
            lab.GetType().GetProperty(propertyName).SetValue(lab, propertyValue);
            return lab;
        }

        private static Lab CreateRandomLab() =>
            CreateLabFiller().Create();

        private static Filler<Lab> CreateLabFiller() =>
            new Filler<Lab>();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);
    }
}
