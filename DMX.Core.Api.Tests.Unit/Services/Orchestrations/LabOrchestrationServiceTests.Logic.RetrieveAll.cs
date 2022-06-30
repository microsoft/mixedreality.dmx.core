// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
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
        public async Task ShouldRetrieveAllLabsWithStatusUpdateAsync()
        {
            // given
            List<Lab> randomLabs =
                CreateRandomLabs(labStatus: LabStatus.Available);

            List<Lab> externalLabs = randomLabs;
            List<Lab> existingLabs = randomLabs.DeepClone();

            List<Lab> additionalExistingLabs =
                CreateRandomLabs(labStatus: LabStatus.Available);

            List<Lab> expectedLabs = existingLabs.DeepClone();

            existingLabs.AddRange(additionalExistingLabs);
            List<Lab> expectedExistingLabs = additionalExistingLabs.DeepClone();

            expectedExistingLabs.ForEach(lab =>
                lab.Status = LabStatus.Offline);

            expectedLabs.AddRange(expectedExistingLabs);

            this.labServiceMock.Setup(service =>
                service.RetrieveAllLabsWithDevices())
                    .Returns(existingLabs.AsQueryable());

            this.externalLabServiceMock.Setup(service =>
                service.RetrieveAllExternalLabsAsync())
                    .ReturnsAsync(externalLabs);

            // when
            List<Lab> actualLabs =
                await this.labOrchestrationService.RetrieveAllLabsAsync();

            // then
            actualLabs.Should().BeEquivalentTo(expectedLabs);

            this.labServiceMock.Verify(service =>
                service.RetrieveAllLabsWithDevices(),
                    Times.Once);

            this.externalLabServiceMock.Verify(service =>
                service.RetrieveAllExternalLabsAsync(),
                    Times.Once);

            this.externalLabServiceMock.VerifyNoOtherCalls();
            this.labServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveAllLabDevicesWithStatusUpdateAsync()
        {
            // given
            Lab randomLab = CreateRandomLab();
            Lab externalLab = randomLab;
            Lab existingLab = randomLab.DeepClone();
            Lab expectedLab = randomLab.DeepClone();
            
            List<LabDevice> commonRandomLabDevices =
                CreateRandomLabDevices(LabDeviceStatus.Online);

            List<LabDevice> expectedLabDevices = commonRandomLabDevices.DeepClone();

            externalLab.Devices = commonRandomLabDevices;
            existingLab.Devices = commonRandomLabDevices.DeepClone();

            List<LabDevice> additionalRandomLabDevices =
                CreateRandomLabDevices(LabDeviceStatus.Online);

            existingLab.Devices.AddRange(additionalRandomLabDevices.DeepClone());

            additionalRandomLabDevices.ForEach(device =>
                device.Status = LabDeviceStatus.Offline);

            expectedLabDevices.AddRange(additionalRandomLabDevices);
            expectedLab.Devices = expectedLabDevices;

            var externalLabs = new List<Lab>() { externalLab };
            var existingLabs = new List<Lab>() { existingLab };
            var expectedLabs = new List<Lab>() { expectedLab };
            
            this.labServiceMock.Setup(service =>
                service.RetrieveAllLabsWithDevices())
                    .Returns(existingLabs.AsQueryable());

            this.externalLabServiceMock.Setup(service =>
                service.RetrieveAllExternalLabsAsync())
                    .ReturnsAsync(externalLabs);

            // when
            List<Lab> actualLabs =
                await this.labOrchestrationService.RetrieveAllLabsAsync();

            // then
            actualLabs.Should().BeEquivalentTo(expectedLabs);

            this.labServiceMock.Verify(service =>
                service.RetrieveAllLabsWithDevices(),
                    Times.Once);

            this.externalLabServiceMock.Verify(service =>
                service.RetrieveAllExternalLabsAsync(),
                    Times.Once);

            this.externalLabServiceMock.VerifyNoOtherCalls();
            this.labServiceMock.VerifyNoOtherCalls();
        }
    }
}