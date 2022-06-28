// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Services.Foundations.ExternalLabs;
using DMX.Core.Api.Services.Foundations.Labs;
using DMX.Core.Api.Services.Orchestrations;
using Moq;
using Tynamix.ObjectFiller;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations
{
    public partial class LabOrchestrationServiceTests
    {
        private readonly Mock<ILabService> labServiceMock;
        private readonly Mock<IExternalLabService> externalLabService;
        private ILabOrchestrationService labOrchestrationService;

        public LabOrchestrationServiceTests()
        {
            this.labServiceMock = new Mock<ILabService>();
            this.externalLabService = new Mock<IExternalLabService>();

            this.labOrchestrationService = new LabOrchestrationService(
                labService: this.labServiceMock.Object,
                externalLabService: this.externalLabService.Object);
        }

        public Filler<Lab> CreateLabsFiller() =>
            new Filler<Lab>();

        public List<Lab> CreateRandomLabsList() =>
            CreateLabsFiller().Create(count: GetRandomNumber()).ToList();

        public IQueryable<Lab> IQueryable() =>
            CreateLabsFiller().Create(count: GetRandomNumber()).AsQueryable();

        public int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}
