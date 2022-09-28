// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.Artifacts;
using DMX.Core.Api.Models.Foundations.Artifacts.Exceptions;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xeptions;
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
    }
}
