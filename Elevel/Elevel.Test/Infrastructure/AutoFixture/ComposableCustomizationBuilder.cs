using AutoFixture;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using System;
using System.Collections.Generic;

namespace Elevel.Test.Infrastructure.AutoFixture
{
    public class ComposableCustomizationBuilder : IFixture
    {
        private readonly IFixture _fixture;

        public ComposableCustomizationBuilder(IFixture fixture)
        {
            _fixture = fixture;
            Builders = new Dictionary<Type, object>();
        }

        public IList<ISpecimenBuilderTransformation> Behaviors => _fixture.Behaviors;

        public IList<ISpecimenBuilder> Customizations => _fixture.Customizations;

        public bool OmitAutoProperties
        {
            get => _fixture.OmitAutoProperties;
            set => _fixture.OmitAutoProperties = value;
        }

        public int RepeatCount
        {
            get => _fixture.RepeatCount;
            set => _fixture.RepeatCount = value;
        }

        public IList<ISpecimenBuilder> ResidueCollectors => _fixture.ResidueCollectors;

        internal Dictionary<Type, object> Builders { get; }

        public ICustomizationComposer<T> Build<T>() => _fixture.Build<T>();

        public object Create(object request, ISpecimenContext context) => _fixture.Create(request, context);

        public IFixture Customize(ICustomization customization)
        {
            customization.Customize(this);
            return this;
        }

        public void Customize<T>(Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation)
        {
            if (!Builders.TryGetValue(typeof(T), out object builder))
            {
                var specimenBuilder = composerTransformation(SpecimenBuilderNodeFactory.CreateComposer<T>().WithAutoProperties(true));
                Builders.Add(typeof(T), new NodeComposer<T>(specimenBuilder));
                return;
            }

            Builders[typeof(T)] = composerTransformation(builder as ICustomizationComposer<T>);
        }

        public IFixture BuildFixture()
        {
            if (_fixture is ComposableCustomizationBuilder)
            {
                return this;
            }

            foreach (var builder in Builders)
            {
                var specimenBuilder = builder.Value as ISpecimenBuilder;
                _fixture.Customizations.Insert(0, specimenBuilder);
            }

            return _fixture;
        }
    }
}
