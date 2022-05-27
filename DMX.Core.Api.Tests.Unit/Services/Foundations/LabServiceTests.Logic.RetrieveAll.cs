// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Models.External.ExternalLabs;
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

            var randomExternalLabsCollection = new ExternalLabsCollection
            {
                Devices = randomLabProperties.Select(randomProperty =>
                    new ExternalLab
                    {
                        Id = randomProperty.Id,
                        Name = randomProperty.Name,
                        IsConnected = randomProperty.IsConnected,
                        IsReserved = randomProperty.IsReserved,
                        Properties = randomProperty.Properties
                    }).ToArray()
            };

            ExternalLabsCollection retrievedExternalLabsCollection =
                randomExternalLabsCollection;

            List<Lab> expectedLabs = randomLabProperties.Select(randomproperty =>
                new Lab
                {
                    ExternalId = randomproperty.Id,
                    Name = randomproperty.Name,
                    Status = randomproperty.LabStatus,
                    Devices = randomproperty.Devices
                }).ToList();

            var externalLabsServiceInformation =
                new ExternalLabsServiceInformation
                {
                    ServiceId = "Bondi-HW-Lab",
                    ServiceType = "AzureIotHub"
                };

            this.labApiBrokerMock.Setup(broker =>
                broker.GetAvailableDevicesAsync(It.Is(
                    SameInformationAs(externalLabsServiceInformation))))
                        .ReturnsAsync(retrievedExternalLabsCollection);

            // when
            List<Lab> actualLabs =
                await this.labService.RetrieveAllLabsAsync();

            // then
            actualLabs.Should().BeEquivalentTo(expectedLabs);

            this.labApiBrokerMock.Verify(broker =>
                broker.GetAvailableDevicesAsync(It.Is(
                    SameInformationAs(externalLabsServiceInformation))),
                        Times.Once);

            this.labApiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
