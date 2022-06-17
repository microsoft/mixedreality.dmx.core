using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace DMX.Core.Api.Tests.Unit.Services.Foundations
{
    public partial class LabServiceTests
    {
        [Fact]
        public async Task ShouldAddLabAsync()
        {
            // given
            var randomLab = CreateRandomLab();

            // when
            var actualLab = 
                await this.labService.AddLabAsync(randomLab);

            // then
            actualLab.Should().NotBeNull();
        }
    }
}
