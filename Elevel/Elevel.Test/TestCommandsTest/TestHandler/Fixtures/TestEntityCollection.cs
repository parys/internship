using AutoFixture;
using Elevel.Domain.Enums;
using Elevel.Test.Infrastructure;
using Elevel.Test.Infrastructure.Customizations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Elevel.Test.TestCommandsTest.TestHandler.Fixtures
{
    [CollectionDefinition(nameof(TestEntityCollection))]
    public class TestEntityCollection : ICollectionFixture<TestEntityFixture> { }


    public class TestEntityFixture : BaseTestFixture
    {
        public TestEntityFixture()
        {
            SeedTestEntities();
        }

        private void SeedTestEntities()
        {
            var tests = new Fixture()
                .Customize(new TestCustomization())
                .CreateMany<Domain.Models.Test>(10)
                .AsQueryable();

            Context.Tests.AddRange(tests);
            Context.SaveChanges();
        }
    }
}
