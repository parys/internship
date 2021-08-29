using AutoFixture;
using Elevel.Test.Infrastructure.FixtureBuiders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Test.Infrastructure.Customizations
{
    public class TestCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoreMembers(new[]{
                nameof(Domain.Models.Test.Essay),
                nameof(Domain.Models.Test.Coach),
                nameof(Domain.Models.Test.Audition),
                nameof(Domain.Models.Test.User),
                nameof(Domain.Models.Test.Hr),
                nameof(Domain.Models.Test.Speaking),
                nameof(Domain.Models.Test.TestQuestions),
                nameof(Domain.Models.Test.Reports)
                }));
        }
    }
}
