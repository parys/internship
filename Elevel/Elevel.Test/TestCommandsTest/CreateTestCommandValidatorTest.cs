﻿using Elevel.Application.Features.TestCommands;
using Elevel.Domain.Enums;
using System;
using System.Collections.Generic;
using Xunit;

namespace Elevel.Test.TestCommandsTest
{
    public class CreateTestCommandValidatorTest
    {
        public static IEnumerable<object[]> UserIds => new List<object[]> 
        {
            new object[]
            {
                Level.Elementary
            },
            new object[]
            {
                Level.PreIntermediate
            },
            new object[]
            {
                 Level.Intermediate
            },
            new object[]
            {
                 Level.Upperintermediate
            },
            new object[]
            {
                Level.Advanced
            }
        };

        [Theory]
        [MemberData(nameof(UserIds))]
        public void CreateTestValidatorTest(Level level)
        {
            //Arrange
            var request = new CreateTestCommand.Request()
            {
                UserId = Guid.NewGuid(),
                Level = level
            };

            //Act

            var validator = new CreateTestCommand.Validator();
            var validatorResult = validator.Validate(request);

            //Assert

            Assert.NotNull(validatorResult);

        }
    }
}
