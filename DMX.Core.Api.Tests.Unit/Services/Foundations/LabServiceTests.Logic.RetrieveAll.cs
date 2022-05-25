// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.ReverbApis;
using DMX.Core.Api.Models.External.ExternalLabs;
using DMX.Core.Api.Services.Foundations;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations
{
    public partial class LabServiceTests
    {
        [Fact]
        public async Task ShouldCallRevertApiAsync()
        {
            // given
            ExternalLabsCollection randomExternalLabCollection =
                CreateRandomLabCollection();

            ExternalLabsCollection retrievedExternalLabCollection =
                randomExternalLabCollection;

            var externalLabsServiceInformation =
                new ExternalLabsServiceInformation
                {
                    ServiceId = "Bondi-HW-Lab",
                    ServiceType = "AzureIotHub"
                };

            this.reverbApiBrokerMock.Setup(broker =>
                broker.GetAvailableDevicesAsync(It.Is(
                    SameInformationAs(externalLabsServiceInformation))))
                        .ReturnsAsync(retrievedExternalLabCollection);

            // when
            await this.labService.RetrieveAllLabsAsync();

            // then
            this.reverbApiBrokerMock.Verify(broker =>
                broker.GetAvailableDevicesAsync(It.Is(
                    SameInformationAs(externalLabsServiceInformation))),
                        Times.Once);

            this.reverbApiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
