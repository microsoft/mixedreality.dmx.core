// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using DMX.Core.Api.Tests.Acceptance.Brokers;
using FluentAssertions;
using Xunit;

namespace DMX.Core.Api.Tests.Acceptance.APIs.Homes
{
    [Collection(nameof(ApiTestCollection))]
    public class HomeApiTests
    {
        private readonly DmxCoreApiBroker dmxCoreApiBroker;

        public HomeApiTests(DmxCoreApiBroker dmxCoreApiBroker) => this.dmxCoreApiBroker = dmxCoreApiBroker;

        [Fact]
        public async Task ShouldReturnHomeMessageAsync()
        {
            // given
            string expectedMessage = "Hi, Zuko here...";

            // when
            string actualMessage = await this.dmxCoreApiBroker.GetHomeMessageAsync();

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);
        }
    }
}
