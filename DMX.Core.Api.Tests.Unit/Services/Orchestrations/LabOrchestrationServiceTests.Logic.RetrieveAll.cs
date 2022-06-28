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
        public async Task ShouldRetrieveSameLabsIfSameLabsAreReturnedFromDependentServicesAsync()
        {
            // given
            List<Lab> randomLabsList = CreateRandomLabsList();
            List<Lab> expectedLabsList = randomLabsList.DeepClone();

            IQueryable<Lab> randomLabsIqueryable =
                randomLabsList.AsQueryable();

            this.labServiceMock.Setup(service =>
                service.RetrieveAllLabs())
                    .Returns(randomLabsIqueryable);

            this.externalLabService.Setup(service =>
                service.RetrieveAllLabsAsync())
                    .ReturnsAsync(randomLabsList);

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
