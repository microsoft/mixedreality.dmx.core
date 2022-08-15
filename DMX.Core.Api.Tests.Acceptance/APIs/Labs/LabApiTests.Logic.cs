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
using RESTFulSense.Exceptions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace DMX.Core.Api.Tests.Acceptance.APIs.Labs
{
    public partial class LabApiTests
    {
        [Fact]
        public async Task ShouldRetrieveAllLabsAsync()
        {
            // given
            List<Lab> randomLabsData = CreateRandomLabsData();

            var retrievedRandomExternalLabCollection = new ExternalLabCollection
            {
                ExternalLabs = randomLabsData.Select(randomProperty =>
                    new ExternalLab
                    {
                        Id = randomProperty.Name,
                        Name = randomProperty.Name,
                        IsConnected = true,
                        IsReserved = false,
                        Properties = new Dictionary<string, string>()
                    }).ToArray()
            };

            List<Lab> randomExternalLabs = randomLabsData.Select(randomProperty =>
                new Lab
                {
                    ExternalId = randomProperty.Name,
                    Name = randomProperty.Name,
                    Status = LabStatus.Unregistered,
                    Devices = new List<LabDevice>(),
                }).ToList();

            List<Lab> retrievedRandomLabs = randomExternalLabs;
            List<Lab> expectedRandomLabs = retrievedRandomLabs.DeepClone();
            Lab randomPostedLab = await PostRandomLabWithoutDevicesAsync();
            randomPostedLab.Devices = new List<LabDevice>();
            randomPostedLab.Status = LabStatus.Offline;
            expectedRandomLabs.Add(randomPostedLab.DeepClone());

            string retrievedExternalRandomLabCollectionBody =
                JsonConvert.SerializeObject(retrievedRandomExternalLabCollection);

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
            await this.dmxCoreApiBroker.DeleteLabByIdAsync(randomPostedLab.Id);
            this.wireMockServer.Stop();
        }

        [Fact]
        public async Task ShouldDeleteLabWithoutDevicesAsync()
        {
            // given
            Lab randomLab = await PostRandomLabWithoutDevicesAsync();
            Lab inputLab = randomLab;
            Lab expectedLab = inputLab;

            // when
            Lab deletedLab =
                await this.dmxCoreApiBroker.DeleteLabByIdAsync(inputLab.Id);

            ValueTask<Lab> getLabByIdTask =
                this.dmxCoreApiBroker.GetLabByIdAsync(inputLab.Id);

            // then
            deletedLab.Should().BeEquivalentTo(expectedLab);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(
                getLabByIdTask.AsTask);

            this.wireMockServer.Stop();
        }
    }
}
