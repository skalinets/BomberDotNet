﻿using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;

namespace Bomberman.Api
{
    public class NSubDataAttribute : AutoDataAttribute
    {
        public NSubDataAttribute() : base(GetFixture())
        {

        }

        public static IFixture GetFixture()
        {
            return new Fixture().Customize(new AutoNSubstituteCustomization());
        }
    }
}