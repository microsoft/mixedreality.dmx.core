// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabArtifacts;
using Xunit;

namespace DMX.Core.Api.Tests.Acceptance.APIs.LabArtifacts
{
    public partial class LabArtifactApiTests
    {
        [Fact]
        public async Task ShouldPostLabArtifactAsync()
        {
            // given
            LabArtifact randomLabArtifact = CreateRandomLabArtifact();
            LabArtifact inputLabArtifact = randomLabArtifact;

            // when .. then
            await this.dmxCoreApiBroker.PostLabArtifactAsync(inputLabArtifact);
        }
    }
}
