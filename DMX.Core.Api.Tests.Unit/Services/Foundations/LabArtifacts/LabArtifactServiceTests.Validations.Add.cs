﻿// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabArtifacts;
using DMX.Core.Api.Models.Foundations.LabArtifacts.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabArtifacts
{
    public partial class LabArtifactServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfArtifactIsInvalidAndLogItAsync(
            string invalidString)
        {
            // given
            string invalidLabArtifactName = invalidString;
            Stream invalidLabArtifactContent = null;

            var invalidArtifactException = new InvalidLabArtifactException();

            invalidArtifactException.AddData(
                key: nameof(LabArtifact.Name),
                values: "Text is required");

            invalidArtifactException.AddData(
                key: nameof(LabArtifact.Content),
                values: "Content is required");

            var expectedArtifactValidationException =
                new LabArtifactValidationException(invalidArtifactException);

            // when
            ValueTask addArtifactTask =
                this.labArtifactService.AddLabArtifactAsync(
                    invalidLabArtifactName,
                    invalidLabArtifactContent);

            LabArtifactValidationException actualArtifactValidationException =
                await Assert.ThrowsAsync<LabArtifactValidationException>(
                    addArtifactTask.AsTask);

            // then
            actualArtifactValidationException.Should()
                .BeEquivalentTo(expectedArtifactValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedArtifactValidationException))),
                        Times.Once);

            this.artifactBroker.Verify(broker =>
                broker.UploadLabArtifactAsync(
                    It.IsAny<LabArtifact>()),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.artifactBroker.VerifyNoOtherCalls();
        }
    }
}
