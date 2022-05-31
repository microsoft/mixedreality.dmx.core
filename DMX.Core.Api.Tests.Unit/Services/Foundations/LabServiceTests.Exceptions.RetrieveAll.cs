// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Externals.ExternalLabs;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using Moq;
using RESTFulSense.Exceptions;
using Xeptions;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations
{
    public partial class LabServiceTests
    {
        [Theory]
        [MemberData(nameof(CriticalDependencyException))]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveIfCriticalErrorOccursAndLogItAsync(
            Xeption criticalDependencyException)
        {
            // given
            var failedExternalLabDependencyException =
                new FailedLabDependencyException(criticalDependencyException);

            var expectedLabDependencyException =
                new LabDependencyException(failedExternalLabDependencyException);

            this.labApiBrokerMock.Setup(broker =>
                broker.GetAvailableLabsAsync(
                    It.IsAny<ExternalLabServiceInformation>()))
                        .ThrowsAsync(criticalDependencyException);

            // when
            ValueTask<List<Lab>> retrieveAllLabsTask =
                this.labService.RetrieveAllLabsAsync();

            // then
            await Assert.ThrowsAsync<LabDependencyException>(() =>
                retrieveAllLabsTask.AsTask());

            this.labApiBrokerMock.Verify(broker =>
                broker.GetAvailableLabsAsync(
                    It.IsAny<ExternalLabServiceInformation>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLabDependencyException))),
                        Times.Once);

            this.labApiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveIfErrorOccursAndLogItAsync()
        {
            // given
            var someResponseMessage = new HttpResponseMessage();
            string someMessage = GetRandomString();
            var httpResponseException = new HttpResponseException(someResponseMessage, someMessage);

            var failedExternalLabDependencyException =
                new FailedLabDependencyException(httpResponseException);

            var expectedLabDependencyException =
                new LabDependencyException(failedExternalLabDependencyException);

            this.labApiBrokerMock.Setup(broker =>
                broker.GetAvailableLabsAsync(
                    It.IsAny<ExternalLabServiceInformation>()))
                        .ThrowsAsync(httpResponseException);

            // when
            ValueTask<List<Lab>> retrieveAllLabsTask =
                this.labService.RetrieveAllLabsAsync();

            // then
            await Assert.ThrowsAsync<LabDependencyException>(() =>
                retrieveAllLabsTask.AsTask());

            this.labApiBrokerMock.Verify(broker =>
                broker.GetAvailableDevicesAsync(
                    It.IsAny<ExternalLabsServiceInformation>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabDependencyException))),
                        Times.Once);

            this.labApiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveIfErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedExternalLabServiceException =
                new FailedLabServiceException(serviceException);

            var expectedLabServiceException =
                new LabServiceException(failedExternalLabServiceException);

            this.labApiBrokerMock.Setup(broker =>
                broker.GetAvailableDevicesAsync(
                    It.IsAny<ExternalLabServiceInformation>()))
                        .ThrowsAsync(serviceException);

            // when
            ValueTask<List<Lab>> retrieveAllLabsTask =
                this.labService.RetrieveAllLabsAsync();

            // then
            await Assert.ThrowsAsync<LabServiceException>(() =>
                retrieveAllLabsTask.AsTask());

            this.labApiBrokerMock.Verify(broker =>
                broker.GetAvailableDevicesAsync(
                    It.IsAny<ExternalLabServiceInformation>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabServiceException))),
                        Times.Once);

            this.labApiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
