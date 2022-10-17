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
        public async Task ShouldAddLabWorkflowAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            LabWorkflow randomLabWorkflow = CreateRandomLabWorkflow(dateTime);
            LabWorkflow inputLabWorkflow = randomLabWorkflow;
            LabWorkflow insertedLabWorkflow = inputLabWorkflow;
            LabWorkflow expectedLabWorkflow = inputLabWorkflow.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLabWorkflowAsync(inputLabWorkflow))
                    .ReturnsAsync(insertedLabWorkflow);

            // when
            LabWorkflow actualLabWorkflow =
                await this.labWorkflowService.AddLabWorkflowAsync(inputLabWorkflow);

            // then
            actualLabWorkflow.Should().BeEquivalentTo(expectedLabWorkflow);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLabWorkflowAsync(inputLabWorkflow),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
