using AutoFixture.Kernel;
using Elevel.Test.Infrastructure.Extensions;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Elevel.Test.Infrastructure.FixtureBuilders
{

    public class LengthConstrained<TEntity> : ISpecimenBuilder
    {
        private readonly int _maxLength;

        private readonly int _minLength;

        private readonly PropertyInfo _prop;

        private readonly Random _rnd = new Random();

        public LengthConstrained(Expression<Func<TEntity, string>> getter, int maxLength, int minLength)
        {
            ThrowsIfInvalid(getter, maxLength, minLength);

            _maxLength = maxLength;
            _minLength = minLength;
            _prop = (PropertyInfo)((MemberExpression)getter.Body).Member;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;

            if (pi != null && pi.AreEquivalent(_prop))
            {
                return GenerateString(_minLength, _maxLength);
            }

            return new NoSpecimen();
        }

        private void ThrowsIfInvalid(Expression<Func<TEntity, string>> getter, int maxLength, int minLength)
        {
            if (maxLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength), "MaxLength should be greater then 0");
            }

            if (minLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength), "MinLength should be greater then 0");
            }

            if (minLength > maxLength)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength), "MinLength should be less then MaxLength");
            }

            var prop = (PropertyInfo)((MemberExpression)getter.Body).Member;
            if (prop.PropertyType != typeof(string))
            {
                throw new NotSupportedException("Only supported for strings");
            }
        }

        private string GenerateString(int minLength, int maxLength)
        {
            const string seed = "acbdefghijklymnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789-()£$";

            var builder = new StringBuilder();

            var length = this._rnd.Next(minLength, maxLength);

            for (var i = 0; i < length; i++)
            {
                var character = seed[this._rnd.Next(0, seed.Length - 1)];
                builder.Append(character);
            }

            return builder.ToString();
        }
    }
}
