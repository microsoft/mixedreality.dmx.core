// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Artifacts;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.Artifacts
{
    public partial class ArtifactServiceTests
    {
        [Fact]
        public async Task ShouldAddArtifactAsync()
        {
            // given
            Artifact randomArtifact = CreateRandomArtifact();
            Artifact inputArtifact = randomArtifact;
            Artifact uploadedArtifact = inputArtifact;

            // when
            await this.artifactService.AddArtifactAsync(inputArtifact);

            // then
            this.artifactBroker.Verify(broker =>
                broker.UploadArtifactAsync(inputArtifact),
                    Times.Once);

            this.artifactBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
