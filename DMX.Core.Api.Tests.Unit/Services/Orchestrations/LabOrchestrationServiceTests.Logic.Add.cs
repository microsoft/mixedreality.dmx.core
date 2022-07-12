// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations
{
    public partial class LabOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldAddLabAsync()
        {
            // given
            Lab randomLab = CreateRandomLab();
            Lab inputLab = randomLab;
            Lab returnedLab = inputLab.DeepClone();
            Lab exptectedLab = returnedLab.DeepClone();

            this.labServiceMock.Setup(service =>
                service.AddLabAsync(inputLab))
                    .ReturnsAsync(returnedLab);

            // when
            Lab actualLab = 
                await this.labOrchestrationService.AddLabAsync(inputLab);

            // then
            actualLab.Should().BeEquivalentTo(exptectedLab);

            this.labServiceMock.Verify(service =>
                service.AddLabAsync(It.IsAny<Lab>()),
                    Times.Once);

            this.labServiceMock.VerifyNoOtherCalls();
            this.externalLabServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
