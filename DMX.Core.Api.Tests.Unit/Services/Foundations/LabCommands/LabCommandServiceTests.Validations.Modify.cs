// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using DMX.Core.Api.Models.Foundations.LabCommands;
using DMX.Core.Api.Models.Foundations.LabCommands.Exceptions;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations.LabCommands
{
    public partial class LabCommandServiceTests
    {
        [Fact]
        public async Task ShouldThrowLabCommandValidationExceptionOnModifyIfInputLabCommandIsNullAndLogItAsync()
        {
            // given
            LabCommand nullLabCommand = null;

            var nullLabCommandException =
                new NullLabCommandException();

            var exptectedLabCommandValidationException =
                new LabCommandValidationException(nullLabCommandException);

            // when
            ValueTask<LabCommand> labCommandTask = 
                this.labCommandService.ModifyLabCommandAsync(nullLabCommand);

            LabCommandValidationException actualLabCommandValidationException =
                await Assert.ThrowsAsync<LabCommandValidationException>(
                    labCommandTask.AsTask);

            // then
            actualLabCommandValidationException.Should().BeEquivalentTo(
                exptectedLabCommandValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    exptectedLabCommandValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLabCommandAsync(It.IsAny<LabCommand>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
