// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.ExternalLabs;
using DMX.Core.Api.Models.Foundations.Labs;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.ExternalLabs
{
    public partial class ExternalLabServiceTest
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

            List<Lab> expectedLabs = randomLabProperties.Select(randomProperty =>
                new Lab
                {
                    ExternalId = randomProperty.Id,
                    Name = randomProperty.Name,
                    Status = randomProperty.LabStatus,
                    Devices = randomProperty.Devices
                }).ToList();

            this.externalLabApiBrokerMock.Setup(broker =>
                broker.GetAvailableLabsAsync())
                    .ReturnsAsync(retrievedExternalLabCollection);

            // when
            List<Lab> actualLabs =
                await this.externalLabService.RetrieveAllExternalLabsAsync();

            // then
            actualLabs.Should().BeEquivalentTo(expectedLabs);

            this.externalLabApiBrokerMock.Verify(broker =>
                broker.GetAvailableLabsAsync(),
                    Times.Once);

            this.externalLabApiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
