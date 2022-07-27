using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task ShouldRetrieveLabFromStorageByIdAsync()
        {
            // given
            Guid randomLabId = Guid.NewGuid();
            Guid inputLabId = randomLabId;
            Lab randomLab = CreateRandomLab();
            Lab selectedLab = randomLab;
            Lab expectedLab = selectedLab.DeepClone();

            this.labServiceMock.Setup(service =>
                service.RetrieveLabByIdAsync(inputLabId))
                    .ReturnsAsync(selectedLab);

            // when
            Lab actualLab =
                await this.labOrchestrationService.RetrieveLabByIdAsync(
                    inputLabId);

            // then
            actualLab.Should().BeEquivalentTo(expectedLab);
            
            this.labServiceMock.Verify(broker =>
                broker.RetrieveLabByIdAsync(inputLabId),
                    Times.Once);

            this.labServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
