// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.ReverbApis;
using DMX.Core.Api.Models.External.ExternalLabs;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Services.Foundations;
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
                        Name = randomProperty.Name
                    }).ToArray()
            };

            ExternalLabsCollection retrievedExternalLabsCollection =
                randomExternalLabsCollection;

            List<Lab> expectedLabs = randomLabProperties.Select(randomproperty =>
                new Lab
                {
                    ExternalId = randomproperty.Id,
                    Name = randomproperty.Name
                }).ToList();

            var externalLabsServiceInformation =
                new ExternalLabsServiceInformation
                {
                    ServiceId = "Bondi-HW-Lab",
                    ServiceType = "AzureIotHub"
                };

            this.reverbApiBrokerMock.Setup(broker =>
                broker.GetAvailableDevicesAsync(It.Is(
                    SameInformationAs(externalLabsServiceInformation))))
                        .ReturnsAsync(retrievedExternalLabsCollection);

            // when
            List<Lab> actualLabs = 
                await this.labService.RetrieveAllLabsAsync();

            // then
            actualLabs.Should().BeEquivalentTo(expectedLabs);

            this.reverbApiBrokerMock.Verify(broker =>
                broker.GetAvailableDevicesAsync(It.Is(
                    SameInformationAs(externalLabsServiceInformation))),
                        Times.Once);

            this.reverbApiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
