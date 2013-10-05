using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;

namespace Bomberman.Api
{
    public class NSubDataAttribute : AutoDataAttribute
    {
        public NSubDataAttribute() : base(new Fixture().Customize(new AutoNSubstituteCustomization()))
        {

        }
    }
}