// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Services.Foundations.ExternalLabs;
using DMX.Core.Api.Services.Foundations.Labs;
using DMX.Core.Api.Services.Orchestrations;
using Force.DeepCloner;
using Moq;
using Tynamix.ObjectFiller;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations
{
    public partial class LabOrchestrationServiceTests
    {
        private readonly Mock<ILabService> labServiceMock;
        private readonly Mock<IExternalLabService> externalLabServiceMock;
        private ILabOrchestrationService labOrchestrationService;

        public LabOrchestrationServiceTests()
        {
            this.labServiceMock = new Mock<ILabService>();
            this.externalLabServiceMock = new Mock<IExternalLabService>();

            this.labOrchestrationService = new LabOrchestrationService(
                labService: this.labServiceMock.Object,
                externalLabService: this.externalLabServiceMock.Object);
        }

        public static List<Lab> CreateRandomLabs(LabStatus labStatus) =>
            CreateLabsFiller(labStatus).Create(count: GetRandomNumber()).ToList();

        public static List<Lab> CreateRandomLabsList() =>
            CreateLabsFiller().Create(count: GetRandomNumber()).ToList();

        public static IQueryable<Lab> IQueryable() =>
            CreateLabsFiller().Create(count: GetRandomNumber()).AsQueryable();

        public static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        public static Filler<Lab> CreateLabsFiller(LabStatus labStatus = LabStatus.Available)
        {
            var filler = new Filler<Lab>();

            filler.Setup()
                .OnProperty(lab => lab.Status).Use(labStatus);

            return filler;
        }
    }
}
