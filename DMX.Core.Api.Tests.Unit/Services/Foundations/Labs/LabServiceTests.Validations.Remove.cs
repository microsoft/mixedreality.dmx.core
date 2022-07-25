using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Foundations.Labs;
using DMX.Core.Api.Models.Foundations.Labs.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.Labs
{
    public partial class LabServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidId = Guid.Empty;
            var invalidLabException = new InvalidLabException();

            invalidLabException.AddData(
                key: nameof(Lab.Id),
                values: "Id is required");

            var expectedLabValidationException =
                new LabValidationException(invalidLabException);

            // when
            ValueTask<Lab> removeLabTask = this.labService.RemoveLabByIdAsync(invalidId);

            LabValidationException actualLabValidationException =
                await Assert.ThrowsAsync<LabValidationException>(
                    removeLabTask.AsTask);

            // then
            actualLabValidationException.Should().BeEquivalentTo(
                expectedLabValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLabByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLabAsync(It.IsAny<Lab>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
