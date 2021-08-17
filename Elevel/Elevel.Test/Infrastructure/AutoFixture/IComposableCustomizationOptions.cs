using AutoFixture;
using System.Collections.Generic;

namespace Elevel.Test.Infrastructure.AutoFixture
{
    public interface IComposableCustomizationOptions
    {
        IList<ICustomization> Customizations { get; }

        IFixture Compose();
    }
}
