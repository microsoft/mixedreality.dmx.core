// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldRemoveLabByIdAsync()
        {
            // given
            Guid inputId = Guid.NewGuid();
            Lab randomLab = CreateRandomLab();
            Lab returnedLab = randomLab.DeepClone();
            Lab expectedLab = randomLab.DeepClone();

            this.labServiceMock.Setup(service =>
                service.RemoveLabByIdAsync(inputId))
                    .ReturnsAsync(returnedLab);

            // when
            Lab actualLab =
                await this.labOrchestrationService.RemoveLabByIdAsync(inputId);

            // then
            actualLab.Should().BeEquivalentTo(expectedLab);

            this.labServiceMock.Verify(service =>
                service.RemoveLabByIdAsync(inputId),
                    Times.Once);

            this.labServiceMock.VerifyNoOtherCalls();
            this.externalLabServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
