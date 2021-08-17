using AutoFixture;
using System.Collections.Generic;

namespace Elevel.Test.Infrastructure.AutoFixture
{
    public class ComposableCustomizationOptions : IComposableCustomizationOptions
    {
        private readonly IFixture _fixture;

        private readonly List<ICustomization> _customizations;

        public IList<ICustomization> Customizations => _customizations;

        public ComposableCustomizationOptions(IFixture fixture, ICustomization customization)
        {
            _customizations = new List<ICustomization> { customization };
            _fixture = fixture;
        }

        public IFixture Compose()
        {
            var builder = _fixture as ComposableCustomizationBuilder ?? new ComposableCustomizationBuilder(_fixture);

            foreach (var customization in _customizations)
            {
                customization.Customize(builder);
            }

            return builder.BuildFixture();
        }
    }
}
