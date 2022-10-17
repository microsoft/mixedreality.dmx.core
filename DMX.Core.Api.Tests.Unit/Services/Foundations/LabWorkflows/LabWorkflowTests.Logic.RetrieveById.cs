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

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabWorkflows
{
    public partial class LabWorkflowTests
    {
        [Fact]
        public async Task ShouldRetrieveLabWorkflowByIdAsync()
        {
            // given
            Guid randomLabWorkflowId = Guid.NewGuid();
            Guid inputLabWorkflowId = randomLabWorkflowId;
            LabWorkflow randomLabWorkflow = CreateRandomLabWorkflow();
            LabWorkflow selectedLabWorkflow = randomLabWorkflow;
            LabWorkflow expectedLabWorkflow = selectedLabWorkflow.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLabWorkflowByIdAsync(inputLabWorkflowId))
                    .ReturnsAsync(selectedLabWorkflow);

            // when
            LabWorkflow actualLabWorkflow =
                await this.labWorkflowService.RetrieveLabWorkflowByIdAsync(inputLabWorkflowId);

            // then
            actualLabWorkflow.Should().BeEquivalentTo(expectedLabWorkflow);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabWorkflowByIdAsync(inputLabWorkflowId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
