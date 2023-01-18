// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabArtifacts;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabArtifacts
{
    public partial class LabArtifactServiceTests
    {
        [Fact]
        public async Task ShouldAddArtifactAsync()
        {
            // given
            LabArtifact randomArtifact = CreateRandomArtifact();
            LabArtifact inputArtifact = randomArtifact;
            LabArtifact uploadedArtifact = inputArtifact;

            // when
            await this.labArtifactService.AddLabArtifactAsync(
                    labArtifactName: inputArtifact.Name,
                    labArtifactContent: inputArtifact.Content);

            // then
            this.artifactBroker.Verify(broker =>
                broker.UploadLabArtifactAsync(
                    It.Is(SameLabArtifactAs(uploadedArtifact))),
                        Times.Once);

            this.artifactBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
