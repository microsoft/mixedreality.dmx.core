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

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.Labs
{
    public partial class LabServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveLabByIdAsync()
        {
            // given
            Guid randomLabId = Guid.NewGuid();
            Guid inputLabId = randomLabId;
            Lab randomLab = CreateRandomLab();
            Lab selectedLab = randomLab;
            Lab expectedLab = selectedLab.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabByIdAsync(inputLabId))
                    .ReturnsAsync(selectedLab);

            // when
            Lab actualLab =
                await this.labService.RetrieveLabByIdAsync(
                    inputLabId);

            // then
            actualLab.Should().BeEquivalentTo(expectedLab);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabByIdAsync(inputLabId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
