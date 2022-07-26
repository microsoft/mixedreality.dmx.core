// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DMX.Core.Api.Tests.Acceptance.Models.Externals.ExternalLabs;
using DMX.Core.Api.Tests.Acceptance.Models.Labs;
using FluentAssertions;
using Force.DeepCloner;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace DMX.Core.Api.Tests.Acceptance.APIs.Labs
{
    public partial class ExternalLabApiTests
    {
        [Fact (Skip = "ExternalLabsController removed. This logic to be reused for LabsController.")]
        public async Task ShouldRetrieveAvailableLabsAsync()
        {
            // given
            List<dynamic> randomLabsData = CreateRandomLabsData();

            var retrievedRandomExternalLabCollection = new ExternalLabCollection
            {
                ExternalLabs = randomLabsData.Select(randomProperty =>
                    new ExternalLab
                    {
                        Id = randomProperty.Id,
                        Name = randomProperty.Name,
                        IsConnected = randomProperty.IsConnected,
                        IsReserved = randomProperty.IsReserved,
                        Properties = randomProperty.Properties
                    }).ToArray()
            };

            string retrievedExternalRandomLabCollectionBody =
                JsonConvert.SerializeObject(retrievedRandomExternalLabCollection);

            List<Lab> randomLabs = randomLabsData.Select(randomProperty =>
                new Lab
                {
                    ExternalId = randomProperty.Id,
                    Name = randomProperty.Name,
                    Status = randomProperty.LabStatus,
                    Devices = randomProperty.Devices,
                }).ToList();

            List<Lab> retrievedRandomLabs = randomLabs;
            List<Lab> expectedRandomLabs = retrievedRandomLabs.DeepClone();

            this.wireMockServer
                .Given(Request.Create()
                    .WithPath("/api/GetAvailableDevicesAsync")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBody(retrievedExternalRandomLabCollectionBody));

            // when
            List<Lab> actualLabs = await this.dmxCoreApiBroker.GetAllLabs();

            // then
            actualLabs.Should().BeEquivalentTo(expectedRandomLabs);
        }
    }
}
