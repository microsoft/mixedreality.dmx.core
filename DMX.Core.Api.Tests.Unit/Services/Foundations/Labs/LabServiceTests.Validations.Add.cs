// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Models.Labs;
using DMX.Core.Api.Models.Labs.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.Labs
{
    public partial class LabServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfNullAndLogItAsync()
        {
            // given
            Lab nullLab = null;

            var nullLabException = new NullLabException();

            var expectedLabValidationException = new LabValidationException(nullLabException);

            // when
            ValueTask<Lab> addLabTask = this.labService.AddLabAsync(nullLab);

            LabValidationException actualLabValidationException =
                await Assert.ThrowsAsync<LabValidationException>(() =>
                    addLabTask.AsTask());

            // then
            actualLabValidationException.Should().BeEquivalentTo(expectedLabValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLabValidationException))),
                        Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
