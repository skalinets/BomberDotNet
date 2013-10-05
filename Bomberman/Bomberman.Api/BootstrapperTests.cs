using FluentAssertions;
using Xunit;

namespace Bomberman.Api
{
    public class BootstrapperTests
    {
        [Fact]
        public void should_create_game()
        {
            new Bootstrapper().GetGame().Should().NotBeNull();

        }
    }
}