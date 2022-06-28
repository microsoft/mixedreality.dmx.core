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

        public static TheoryData CreateLabsData()
        {
            Lab randomLab = CreateRandomLab();
            return new TheoryData<List<Lab>, IQueryable<Lab>, List<Lab>>()
            {
                {
                    new List<Lab>() { randomLab },
                    new List<Lab>() { randomLab }.AsQueryable(),
                    new List<Lab>() { CloneLabAndSetStatus(randomLab, LabStatus.Available) }
                },
                {
                    new List<Lab>() { },
                    new List<Lab>() { randomLab }.AsQueryable(),
                    new List<Lab>() { CloneLabAndSetStatus(randomLab, LabStatus.Offline) }
                },
                {
                    new List<Lab>() { randomLab },
                    new List<Lab>() { }.AsQueryable(),
                    new List<Lab>() { }
                },
                {
                    new List<Lab>() { },
                    new List<Lab>() { }.AsQueryable(),
                    new List<Lab>() { }
                },
            };
        }

        private static Lab CloneLabAndSetStatus(Lab lab, LabStatus status)
        {
            Lab clonedLab = lab.DeepClone();
            clonedLab.Status = status;
            return clonedLab;
        }

        public static Lab CreateRandomLab() =>
            CreateLabsFiller().Create();

        public static List<Lab> CreateRandomLabsList() =>
            CreateLabsFiller().Create(count: GetRandomNumber()).ToList();

        public static IQueryable<Lab> IQueryable() =>
            CreateLabsFiller().Create(count: GetRandomNumber()).AsQueryable();

        public static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        public static Filler<Lab> CreateLabsFiller() =>
            new Filler<Lab>();
    }
}
