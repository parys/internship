using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Test.Infrastructure.FixtureBuiders
{
    public class IgnoreMembers : ISpecimenBuilder
    {

        public IEnumerable<string> Members { get; }


        public IgnoreMembers(IEnumerable<string> members)
        {
            Members = members ?? new List<string>();
        }

        public object Create(object request, ISpecimenContext context)
        {
            var propertyInfo = request as PropertyInfo;

            if (propertyInfo == null)
            {
                return new NoSpecimen();
            }

            if (Members.ToList().Contains(propertyInfo.Name))
            {
                return null;
            }

            return new NoSpecimen();
        }
    }
}
