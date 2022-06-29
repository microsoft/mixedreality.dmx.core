// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Models.ExternalLabs.Exceptions;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using DMX.Core.Api.Services.Foundations.ExternalLabs;
using DMX.Core.Api.Services.Foundations.Labs;
using DMX.Core.Api.Services.Orchestrations;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations
{
    public partial class LabOrchestrationServiceTests
    {
        private readonly Mock<ILabService> labServiceMock;
        private readonly Mock<IExternalLabService> externalLabServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private ILabOrchestrationService labOrchestrationService;

        public LabOrchestrationServiceTests()
        {
            this.labServiceMock = new Mock<ILabService>();
            this.externalLabServiceMock = new Mock<IExternalLabService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.labOrchestrationService = new LabOrchestrationService(
                labService: this.labServiceMock.Object,
                externalLabService: this.externalLabServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomErrorMessage = GetRandomString();
            var innerException = new Xeption(randomErrorMessage);

            return new TheoryData<Xeption>
            {
                new ExternalLabDependencyException(innerException),
                new ExternalLabServiceException(innerException),
                new LabDependencyException(innerException),
                new LabServiceException(innerException)
            };
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        public static List<Lab> CreateRandomLabs(LabStatus labStatus) =>
            CreateLabsFiller(labStatus).Create(count: GetRandomNumber()).ToList();

        public static List<Lab> CreateRandomLabsList() =>
            CreateLabsFiller().Create(count: GetRandomNumber()).ToList();

        public static IQueryable<Lab> IQueryable() =>
            CreateLabsFiller().Create(count: GetRandomNumber()).AsQueryable();

        public static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        public static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        public static Filler<Lab> CreateLabsFiller(LabStatus labStatus = LabStatus.Available)
        {
            var filler = new Filler<Lab>();

            filler.Setup()
                .OnProperty(lab => lab.Status).Use(labStatus);

            return filler;
        }
    }
}
