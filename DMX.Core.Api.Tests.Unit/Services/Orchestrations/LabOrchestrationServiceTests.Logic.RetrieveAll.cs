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
        [Fact]
        public async Task ShouldRetrieveAllLabsWithStatusUpdateAsync()
        {
            // given
            List<Lab> randomLabs =
                CreateRandomLabs(labStatus: LabStatus.Available);

            List<Lab> externalLabs = randomLabs;
            List<Lab> existingLabs = randomLabs.DeepClone();

            List<Lab> additionalExistingLabs =
                CreateRandomLabs(labStatus: LabStatus.Available);

            existingLabs.AddRange(additionalExistingLabs);
            List<Lab> expectedExistingLabs = additionalExistingLabs.DeepClone();

            expectedExistingLabs.ForEach(lab =>
                lab.Status = LabStatus.Offline);

            List<Lab> expectedLabs = existingLabs.DeepClone();
            expectedLabs.AddRange(expectedExistingLabs);

            this.labServiceMock.Setup(service =>
                service.RetrieveAllLabs())
                    .Returns(existingLabs.AsQueryable());

            this.externalLabServiceMock.Setup(service =>
                service.RetrieveAllLabsAsync())
                    .ReturnsAsync(externalLabs);

            // when
            List<Lab> actualLabs =
                await this.labOrchestrationService.RetrieveAllLabsAsync();

            // then
            actualLabs.Should().BeEquivalentTo(expectedLabs);

            this.labServiceMock.Verify(service =>
                service.RetrieveAllLabs(),
                    Times.Once);

            this.externalLabServiceMock.Verify(service =>
                service.RetrieveAllLabsAsync(),
                    Times.Once);

            this.externalLabServiceMock.VerifyNoOtherCalls();
            this.labServiceMock.VerifyNoOtherCalls();
        }
    }
}