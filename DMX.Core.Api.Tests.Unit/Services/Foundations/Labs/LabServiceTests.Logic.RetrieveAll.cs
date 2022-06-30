// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
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
        public void ShouldRetrieveLabs()
        {
            // given
            IQueryable<Lab> randomLabs = CreateRandomLabs();
            IQueryable<Lab> retrievedLabs = randomLabs;
            IQueryable<Lab> expectedLabs = randomLabs.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllLabsWithDevices())
                    .Returns(retrievedLabs);

            // when
            IQueryable<Lab> actualLabs =
                this.labService.RetrieveAllLabsWithDevices();

            // then
            actualLabs.Should().BeEquivalentTo(expectedLabs);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllLabsWithDevices(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
