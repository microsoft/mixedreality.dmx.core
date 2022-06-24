// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMX.Core.Api.Tests.Acceptance.Models.Labs;
using FluentAssertions;
using Force.DeepCloner;
using Xunit;

namespace DMX.Core.Api.Tests.Acceptance.APIs.Labs
{
    public partial class LabApiTests
    {
        [Fact]
        public async Task ShouldAddLabAsync()
        {
            // given
            Lab randomLab = CreateRandomLab();

            randomLab.Devices = new List<LabDevice>();

            Lab inputLab = randomLab;
            Lab expectedLab = inputLab.DeepClone();

            // when
            Lab actualLab = 
                await this.dmxCoreApiBroker.PostLabAsync(inputLab);

            // then
            actualLab.Should().BeEquivalentTo(expectedLab);
        }
    }
}
