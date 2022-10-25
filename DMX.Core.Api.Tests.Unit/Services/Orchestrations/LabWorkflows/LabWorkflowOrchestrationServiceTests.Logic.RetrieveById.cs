// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.LabWorkflows;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Orchestrations.LabWorkflows
{
    public partial class LabWorkflowOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveLabWorkflowFromStorageByIdAsync()
        {
            // given
            Guid randomLabWorkflowId = Guid.NewGuid();
            Guid inputLabWorkflowId = randomLabWorkflowId;
            LabWorkflow randomLabWorkflow = CreateRandomLabWorkflow();
            LabWorkflow retrievedLabWorkflow = randomLabWorkflow;
            LabWorkflow expectedLabWorkflow = retrievedLabWorkflow.DeepClone();

            this.labWorkflowServiceMock.Setup(service =>
                service.RetrieveLabWorkflowByIdAsync(inputLabWorkflowId))
                    .ReturnsAsync(retrievedLabWorkflow);

            // when
            LabWorkflow actualLabWorkflow =
                await this.labWorkflowOrchestrationService
                    .RetrieveLabWorkflowByIdAsync(inputLabWorkflowId);

            // then
            actualLabWorkflow.Should().BeEquivalentTo(expectedLabWorkflow);

            this.labWorkflowServiceMock.Verify(service =>
                service.RetrieveLabWorkflowByIdAsync(
                    inputLabWorkflowId),
                        Times.Once);

            this.labWorkflowServiceMock.VerifyNoOtherCalls();
            this.labWorkflowCommandServiceMock.VerifyNoOtherCalls();
            this.labWorkflowEventServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
