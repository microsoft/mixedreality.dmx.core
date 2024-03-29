﻿// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Orchestrations.Labs.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations.Labs
{
    public partial class LabOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfLabIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidLabId = Guid.Empty;
            var invalidLabOrchestrationException = new InvalidLabOrchestrationException();

            invalidLabOrchestrationException.AddData(
                key: nameof(Lab.Id),
                values: "Id is required");

            var expectedLabOrchestrationValidationException =
                new LabOrchestrationValidationException(invalidLabOrchestrationException);

            // when
            ValueTask<Lab> actualLabTask =
                this.labOrchestrationService.RemoveLabByIdAsync(invalidLabId);

            LabOrchestrationValidationException
                actualLabOrchestrationValidationException =
                    await Assert.ThrowsAsync<LabOrchestrationValidationException>(
                        actualLabTask.AsTask);

            // then
            actualLabOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedLabOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabOrchestrationValidationException))),
                        Times.Once);

            this.labServiceMock.Verify(service =>
                service.RemoveLabByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.labServiceMock.VerifyNoOtherCalls();
            this.externalLabServiceMock.VerifyNoOtherCalls();
        }
    }
}
