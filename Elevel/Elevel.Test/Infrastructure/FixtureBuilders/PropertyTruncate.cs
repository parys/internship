using AutoFixture;
using AutoFixture.Kernel;
using Elevel.Test.Infrastructure.Extensions;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Elevel.Test.Infrastructure.FixtureBuilders
{
    public class PropertyTruncate<TEntity> : ISpecimenBuilder
    {
        private readonly int _length;

        private readonly PropertyInfo _prop;

        public PropertyTruncate(Expression<Func<TEntity, string>> getter, int length)
        {
            ThrowsIfInvalid(getter);

            _length = length;
            _prop = (PropertyInfo)((MemberExpression)getter.Body).Member;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;

            if (pi != null && pi.AreEquivalent(_prop))
            {
                return context.Create<string>().Substring(0, _length);
            }

            return new NoSpecimen();
        }

        private static void ThrowsIfInvalid(Expression<Func<TEntity, string>> getter)
        {
            var prop = (PropertyInfo)((MemberExpression)getter.Body).Member;

            if (prop.PropertyType != typeof(string))
            {
                throw new NotSupportedException("Only supported for strings");
            }
        }
    }
}
