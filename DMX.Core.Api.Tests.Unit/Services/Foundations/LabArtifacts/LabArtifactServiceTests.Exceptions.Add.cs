// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Net;
using System.Threading.Tasks;
using Azure;
using DMX.Core.Api.Models.Foundations.LabArtifacts;
using DMX.Core.Api.Models.Foundations.LabArtifacts.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabArtifacts
{
    public partial class LabArtifactServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync()
        {
            // given
            LabArtifact someArtifact = CreateRandomArtifact();
            string randomMessage = GetRandomString();
            var requestFailedException = new RequestFailedException(randomMessage);

            var failedArtifactDependencyException =
                new FailedLabArtifactDependencyException(requestFailedException);

            var expectedArtifactDependencyException =
                new LabArtifactDependencyException(failedArtifactDependencyException);

            this.artifactBroker.Setup(broker =>
                broker.UploadLabArtifactAsync(It.IsAny<LabArtifact>()))
                    .Throws(requestFailedException);

            // when
            ValueTask uploadArtifactTask =
                this.labArtifactService.AddLabArtifactAsync(someArtifact);

            LabArtifactDependencyException actualArtifactDependencyException =
                await Assert.ThrowsAsync<LabArtifactDependencyException>(
                    uploadArtifactTask.AsTask);

            // then
            actualArtifactDependencyException.Should().BeEquivalentTo(
                expectedArtifactDependencyException);

            this.artifactBroker.Verify(broker =>
                broker.UploadLabArtifactAsync(It.IsAny<LabArtifact>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedArtifactDependencyException))),
                        Times.Once);

            this.artifactBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData((int)HttpStatusCode.Unauthorized)]
        [InlineData((int)HttpStatusCode.Forbidden)]
        [InlineData((int)HttpStatusCode.NotFound)]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfCriticalErrorOccursAndLogItAsync(
            int crititicalStatusCode)
        {
            // given
            LabArtifact someArtifact = CreateRandomArtifact();
            string randomMessage = GetRandomString();

            var requestFailedException =
                new RequestFailedException(
                    crititicalStatusCode,
                    randomMessage);

            var failedArtifactDependencyException =
                new FailedLabArtifactDependencyException(requestFailedException);

            var expectedArtifactDependencyException =
                new LabArtifactDependencyException(failedArtifactDependencyException);

            this.artifactBroker.Setup(broker =>
                broker.UploadLabArtifactAsync(It.IsAny<LabArtifact>()))
                    .Throws(requestFailedException);

            // when
            ValueTask uploadArtifactTask =
                this.labArtifactService.AddLabArtifactAsync(someArtifact);

            LabArtifactDependencyException actualArtifactDependencyException =
                await Assert.ThrowsAsync<LabArtifactDependencyException>(
                    uploadArtifactTask.AsTask);

            // then
            actualArtifactDependencyException.Should().BeEquivalentTo(
                expectedArtifactDependencyException);

            this.artifactBroker.Verify(broker =>
                broker.UploadLabArtifactAsync(It.IsAny<LabArtifact>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedArtifactDependencyException))),
                        Times.Once);

            this.artifactBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfLabArtifactAlreadyExistsAndLogItAsync()
        {
            // given
            LabArtifact someLabArtifact = CreateRandomArtifact();

            string randomMessage = GetRandomString();

            var requestFailedConflictException =
                new RequestFailedException(
                    (int)HttpStatusCode.Conflict,
                    randomMessage);

            var alreadyExistsLabArtifactException =
                new AlreadyExistsLabArtifactException(requestFailedConflictException);

            var expectedLabArtifactDependencyValidationException =
                new LabArtifactDependencyValidationException(alreadyExistsLabArtifactException);

            this.artifactBroker.Setup(broker =>
                broker.UploadLabArtifactAsync(It.IsAny<LabArtifact>()))
                    .Throws(requestFailedConflictException);

            // when
            ValueTask addLabArtifactTask =
                this.labArtifactService.AddLabArtifactAsync(someLabArtifact);

            LabArtifactDependencyValidationException actualLabArtifactDependencyValidationException =
                await Assert.ThrowsAsync<LabArtifactDependencyValidationException>(
                    addLabArtifactTask.AsTask);

            // then
            actualLabArtifactDependencyValidationException.Should().BeEquivalentTo(
                expectedLabArtifactDependencyValidationException);

            this.artifactBroker.Verify(broker =>
                broker.UploadLabArtifactAsync(
                    It.IsAny<LabArtifact>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabArtifactDependencyValidationException))),
                        Times.Once);

            this.artifactBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfErrorOccursAndLogItAsync()
        {
            // given
            LabArtifact randomArtifact = CreateRandomArtifact();
            LabArtifact someArtifact = randomArtifact;
            var serviceException = new Exception();

            var failedArtifactServiceException =
                new FailedLabArtifactServiceException(serviceException);

            var expectedArtifactServiceException =
                new LabArtifactServiceException(failedArtifactServiceException);

            this.artifactBroker.Setup(broker =>
                broker.UploadLabArtifactAsync(It.IsAny<LabArtifact>()))
                    .Throws(serviceException);

            // when
            ValueTask addArtifactTask =
                this.labArtifactService.AddLabArtifactAsync(someArtifact);

            LabArtifactServiceException actualArtifactServiceException =
                await Assert.ThrowsAsync<LabArtifactServiceException>(
                    addArtifactTask.AsTask);

            // then
            actualArtifactServiceException.Should().BeEquivalentTo(
                expectedArtifactServiceException);

            this.artifactBroker.Verify(broker =>
                broker.UploadLabArtifactAsync(It.IsAny<LabArtifact>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedArtifactServiceException))),
                        Times.Once);

            this.artifactBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
