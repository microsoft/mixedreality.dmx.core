// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
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
        public async Task ShouldThrowValidationExceptionOnAddIfNullAndLogItAsync()
        {
            // given
            Artifact nullArtifact = null;
            var nullArtifactException = new NullArtifactException();

            var exptectedArtifactValidationException =
                new ArtifactValidationException(nullArtifactException);

            // when
            ValueTask addArtifactTask =
                this.artifactService.AddArtifactAsync(nullArtifact);

            ArtifactValidationException actualArtifactValidationException =
                await Assert.ThrowsAsync<ArtifactValidationException>(
                    addArtifactTask.AsTask);

            // then
            actualArtifactValidationException.Should().BeEquivalentTo(
                exptectedArtifactValidationException);

            this.loggingBrokerMock.Verify(brokers =>
                brokers.LogError(It.Is(SameExceptionAs(
                    exptectedArtifactValidationException))),
                        Times.Once);

            this.artifactBroker.Verify(broker =>
                broker.UploadArtifactAsync(It.IsAny<Artifact>()),
                    Times.Never);

            this.artifactBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfArtifactIsInvalidAndLogItAsync(
            string invalidString)
        {
            // given
            var invalidArtifact = new Artifact
            {
                Name = invalidString,
                Content = null
            };

            var invalidArtifactException = new InvalidArtifactException();

            invalidArtifactException.AddData(
                key: nameof(Artifact.Name),
                values: "Text is required");

            invalidArtifactException.AddData(
                key: nameof(Artifact.Content),
                values: "Content is required");

            var expectedArtifactValidationException =
                new ArtifactValidationException(invalidArtifactException);

            // when
            ValueTask addArtifactTask =
                this.artifactService.AddArtifactAsync(invalidArtifact);

            ArtifactValidationException actualArtifactValidationException =
                await Assert.ThrowsAsync<ArtifactValidationException>(
                    addArtifactTask.AsTask);

            // then
            actualArtifactValidationException.Should()
                .BeEquivalentTo(expectedArtifactValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedArtifactValidationException))),
                        Times.Once);

            this.artifactBroker.Verify(broker =>
                broker.UploadArtifactAsync(
                    It.IsAny<Artifact>()),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.artifactBroker.VerifyNoOtherCalls();
        }
    }
}
