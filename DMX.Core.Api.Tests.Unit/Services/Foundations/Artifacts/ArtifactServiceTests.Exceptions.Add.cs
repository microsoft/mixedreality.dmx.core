// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Azure;
using DMX.Core.Api.Models.Foundations.Artifacts;
using DMX.Core.Api.Models.Foundations.Artifacts.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.Artifacts
{
    public partial class ArtifactServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync()
        {
            // given
            Artifact someArtifact = CreateRandomArtifact();
            string randomMessage = GetRandomString();
            var requestFailedException = new RequestFailedException(randomMessage);

            var failedArtifactDependencyException =
                new FailedArtifactDependencyException(requestFailedException);

            var expectedArtifactDependencyException =
                new ArtifactDependencyException(failedArtifactDependencyException);

            this.artifactBroker.Setup(broker =>
                broker.UploadArtifactAsync(It.IsAny<Artifact>()))
                    .Throws(requestFailedException);

            // when
            ValueTask uploadArtifactTask =
                this.artifactService.AddArtifactAsync(someArtifact);

            ArtifactDependencyException actualArtifactDependencyException =
                await Assert.ThrowsAsync<ArtifactDependencyException>(
                    uploadArtifactTask.AsTask);

            // then
            actualArtifactDependencyException.Should().BeEquivalentTo(
                expectedArtifactDependencyException);

            this.artifactBroker.Verify(broker =>
                broker.UploadArtifactAsync(It.IsAny<Artifact>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedArtifactDependencyException))),
                        Times.Once);

            this.artifactBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfErrorOccursAndLogItAsync()
        {
            // given
            Artifact randomArtifact = CreateRandomArtifact();
            Artifact someArtifact = randomArtifact;
            var serviceException = new Exception();

            var failedArtifactServiceException =
                new FailedArtifactServiceException(serviceException);

            var expectedArtifactServiceException =
                new ArtifactServiceException(failedArtifactServiceException);

            this.artifactBroker.Setup(broker =>
                broker.UploadArtifactAsync(It.IsAny<Artifact>()))
                    .Throws(serviceException);

            // when
            ValueTask addArtifactTask =
                this.artifactService.AddArtifactAsync(someArtifact);

            ArtifactServiceException actualArtifactServiceException =
                await Assert.ThrowsAsync<ArtifactServiceException>(
                    addArtifactTask.AsTask);

            // then
            actualArtifactServiceException.Should().BeEquivalentTo(
                expectedArtifactServiceException);

            this.artifactBroker.Verify(broker =>
                broker.UploadArtifactAsync(It.IsAny<Artifact>()),
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
