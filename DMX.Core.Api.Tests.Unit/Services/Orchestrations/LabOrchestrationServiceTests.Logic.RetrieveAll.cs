// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Labs;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations
{
    public partial class LabOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(CreateLabsData))]
        public async Task ShouldRetrieveLabsWithStatusAsync(
            List<Lab> externalLabList,
            IQueryable<Lab> labQuerable,
            List<Lab> expectedLabsList)
        {
            // given
            this.externalLabService.Setup(service =>
                service.RetrieveAllLabsAsync())
                    .ReturnsAsync(externalLabList);

            this.labServiceMock.Setup(service =>
                service.RetrieveAllLabs())
                    .Returns(labQuerable);

            // when
            List<Lab> actualLabs =
                await this.labOrchestrationService.RetrieveAllLabsAsync();

            // then
            actualLabs.Should().BeEquivalentTo(expectedLabsList);

            this.labServiceMock.Verify(service =>
                service.RetrieveAllLabs(),
                    Times.Once);

            this.externalLabService.Verify(service =>
                service.RetrieveAllLabsAsync(),
                    Times.Once);

            this.labServiceMock.VerifyNoOtherCalls();
            this.externalLabService.VerifyNoOtherCalls();
        }
    }
}
