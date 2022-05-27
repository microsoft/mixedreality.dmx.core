// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Externals.ExternalLabs;
using DMX.Core.Api.Models.Labs;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations
{
    public partial class LabServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveLabsAsync()
        {
            // given
            List<dynamic> randomLabProperties = CreateRandomLabsProperties();

            var randomExternalLabCollection = new ExternalLabCollection
            {
                ExternalLabs = randomLabProperties.Select(randomProperty =>
                    new ExternalLab
                    {
                        Id = randomProperty.Id,
                        Name = randomProperty.Name,
                        IsConnected = randomProperty.IsConnected,
                        IsReserved = randomProperty.IsReserved,
                        Properties = randomProperty.Properties
                    }).ToArray()
            };

            ExternalLabCollection retrievedExternalLabCollection =
                randomExternalLabCollection;

            List<Lab> expectedLabs = randomLabProperties.Select(randomproperty =>
                new Lab
                {
                    ExternalId = randomproperty.Id,
                    Name = randomproperty.Name,
                    Status = randomproperty.LabStatus,
                    Devices = randomproperty.Devices
                }).ToList();

            var externalLabServiceInformation =
                new ExternalLabServiceInformation
                {
                    ServiceId = "Bondi-HW-Lab",
                    ServiceType = "AzureIotHub"
                };

            this.reverbApiBrokerMock.Setup(broker =>
                broker.GetAvailableLabsAsync(It.Is(
                    SameInformationAs(externalLabServiceInformation))))
                        .ReturnsAsync(retrievedExternalLabCollection);

            // when
            List<Lab> actualLabs =
                await this.labService.RetrieveAllLabsAsync();

            // then
            actualLabs.Should().BeEquivalentTo(expectedLabs);

            this.reverbApiBrokerMock.Verify(broker =>
                broker.GetAvailableLabsAsync(It.Is(
                    SameInformationAs(externalLabServiceInformation))),
                        Times.Once);

            this.reverbApiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
