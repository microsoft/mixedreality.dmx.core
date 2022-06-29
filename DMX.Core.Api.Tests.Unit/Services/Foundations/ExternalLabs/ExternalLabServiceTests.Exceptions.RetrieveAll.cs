// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.ExternalLabs;
using DMX.Core.Api.Models.Foundations.ExternalLabs.Exceptions;
using DMX.Core.Api.Models.Foundations.Labs;
using FluentAssertions;
using Moq;
using RESTFulSense.Exceptions;
using Xeptions;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.ExternalLabs
{
    public partial class ExternalLabServiceTest
    {
        [Theory]
        [MemberData(nameof(CriticalDependencyException))]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveIfCriticalErrorOccursAndLogItAsync(
            Xeption criticalDependencyException)
        {
            // given
            var failedExternalLabDependencyException =
                new FailedExternalLabDependencyException(criticalDependencyException);

            var expectedLabDependencyException =
                new ExternalLabDependencyException(failedExternalLabDependencyException);

            this.externalLabApiBrokerMock.Setup(broker =>
                broker.GetAvailableLabsAsync(
                    It.IsAny<ExternalLabServiceInformation>()))
                        .ThrowsAsync(criticalDependencyException);

            // when
            ValueTask<List<Lab>> retrieveAllLabsTask =
                this.externalLabService.RetrieveAllExternalLabsAsync();

            ExternalLabDependencyException actualLabDependencyException =
                await Assert.ThrowsAsync<ExternalLabDependencyException>(() =>
                    retrieveAllLabsTask.AsTask());

            // then
            actualLabDependencyException.Should().BeEquivalentTo(
                expectedLabDependencyException);

            this.externalLabApiBrokerMock.Verify(broker =>
                broker.GetAvailableLabsAsync(
                    It.IsAny<ExternalLabServiceInformation>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLabDependencyException))),
                        Times.Once);

            this.externalLabApiBrokerMock.VerifyNoOtherCalls();
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
                new FailedExternalLabDependencyException(httpResponseException);

            var expectedLabDependencyException =
                new ExternalLabDependencyException(failedExternalLabDependencyException);

            this.externalLabApiBrokerMock.Setup(broker =>
                broker.GetAvailableLabsAsync(
                    It.IsAny<ExternalLabServiceInformation>()))
                        .ThrowsAsync(httpResponseException);

            // when
            ValueTask<List<Lab>> retrieveAllLabsTask =
                this.externalLabService.RetrieveAllExternalLabsAsync();

            ExternalLabDependencyException actualExternalLabDependencyException =
                await Assert.ThrowsAsync<ExternalLabDependencyException>(() =>
                    retrieveAllLabsTask.AsTask());

            // then
            actualExternalLabDependencyException.Should().BeEquivalentTo(
                expectedLabDependencyException);

            this.externalLabApiBrokerMock.Verify(broker =>
                broker.GetAvailableLabsAsync(
                    It.IsAny<ExternalLabServiceInformation>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabDependencyException))),
                        Times.Once);

            this.externalLabApiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveIfErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedExternalLabServiceException =
                new FailedExternalLabServiceException(serviceException);

            var expectedLabServiceException =
                new ExternaLabServiceException(failedExternalLabServiceException);

            this.externalLabApiBrokerMock.Setup(broker =>
                broker.GetAvailableLabsAsync(
                    It.IsAny<ExternalLabServiceInformation>()))
                        .ThrowsAsync(serviceException);

            // when
            ValueTask<List<Lab>> retrieveAllLabsTask =
                this.externalLabService.RetrieveAllExternalLabsAsync();

            ExternaLabServiceException actualLabServiceException =
                await Assert.ThrowsAsync<ExternaLabServiceException>(() =>
                    retrieveAllLabsTask.AsTask());

            // then
            actualLabServiceException.Should().BeEquivalentTo(
                expectedLabServiceException);

            this.externalLabApiBrokerMock.Verify(broker =>
                broker.GetAvailableLabsAsync(
                    It.IsAny<ExternalLabServiceInformation>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabServiceException))),
                        Times.Once);

            this.externalLabApiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
