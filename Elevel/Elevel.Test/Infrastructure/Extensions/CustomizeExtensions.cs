using AutoFixture;
using System;

namespace Elevel.Test.Infrastructure.Extensions
{
    public static class CustomizeExtensions
    {
        public static IFixture Customize<T>(this IFixture fixture) where T : ICustomization, new()
        {
            fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));

            return fixture.Customize(Activator.CreateInstance<T>());
        }
    }
}
