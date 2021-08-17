using AutoFixture;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using Elevel.Test.Infrastructure.AutoFixture;
using System;

namespace Elevel.Test.Infrastructure.Extensions
{
    public static class ComposeExtensions
    {
        public static IComposableCustomizationOptions Compose<T>(this IFixture fixture) where T : ICustomization
        {
            fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));

            return new ComposableCustomizationOptions(fixture, Activator.CreateInstance<T>());
        }

        public static T Create<T>(this IComposableCustomizationOptions options) where T : new()
        {
            options = options ?? throw new ArgumentNullException(nameof(options));

            return options.Compose().Create<T>();
        }

        public static IComposableCustomizationOptions Compose(this IFixture fixture, ICustomization customization)
        {
            fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));

            return new ComposableCustomizationOptions(fixture, customization);
        }

        public static IComposableCustomizationOptions Compose<T>(this IFixture fixture, Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation)
        {
            fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));

            composerTransformation = composerTransformation ?? throw new ArgumentNullException(nameof(composerTransformation));

            return new ComposableCustomizationOptions(fixture, new ComposeCustomization<T>(composerTransformation));
        }

        public static IComposableCustomizationOptions Compose<T>(this IComposableCustomizationOptions options) where T : ICustomization
        {
            options = options ?? throw new ArgumentNullException(nameof(options));

            options.Customizations.Add(Activator.CreateInstance<T>());

            return options;
        }

        public static IComposableCustomizationOptions Compose<T>(this IComposableCustomizationOptions options, Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation)
        {
            options = options ?? throw new ArgumentNullException(nameof(options));

            composerTransformation = composerTransformation ?? throw new ArgumentNullException(nameof(composerTransformation));

            options.Customizations.Add(new ComposeCustomization<T>(composerTransformation));

            return options;
        }
    }
}
