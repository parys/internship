using System;
using System.Collections.Generic;
using Elevel.Application.Features.ReportCommands;
using Xunit;

namespace Elevel.Test.ReportCommandTest.CreateReport
{
    public class CreateReportCommandValidatorTests
    {
        public static IEnumerable<object[]> NullOrEmptyGuidFalseData =>
            new List<object[]>
            {
                new object[] { Guid.Empty, Guid.NewGuid(), Guid.Empty, "When AuditionId is given, QuestionId and TopicId must be empty." },
                new object[] { Guid.Empty, null, Guid.Empty, "At least one field QuestionId, AuditionId or TopicId must have a value." }
            };

        public static IEnumerable<object[]> NullOrEmptyGuidTrueData =>
            new List<object[]>
            {
                new object[] { Guid.NewGuid(), Guid.Empty, null },
                new object[] { Guid.NewGuid(), null, null }
            };

        [Theory]
        [MemberData(nameof(NullOrEmptyGuidFalseData))]
        public void QuestionIdAndAuditionIdAndTopicIdCouldNotBeNullOrEmptyGuidFalseTest(Guid? questionId, Guid? auditionId,
            Guid? topicId, string messageAboutError)
        {
            // Arrange
            var request = new CreateReportCommand.Request()
            {
                QuestionId = questionId,
                AuditionId = auditionId,
                TopicId = topicId,
                TestId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Description = Guid.NewGuid().ToString()
            };

            // Act
            var validator = new CreateReportCommand.Validator();
            var validationResult = validator.Validate(request);

            // Assert
            Assert.NotNull(validationResult);
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(validationResult.Errors);
            Assert.Single(validationResult.Errors);
            Assert.Equal(messageAboutError,
                validationResult.Errors[0].ErrorMessage);
        }

        [Theory]
        [MemberData(nameof(NullOrEmptyGuidTrueData))]
        public void QuestionIdAndAuditionIdAndTopicIdCouldNotBeNullOrEmptyGuidTrueTest(Guid? questionId, Guid? auditionId, Guid? topicId)
        {
            //Arrange
            var request = new CreateReportCommand.Request()
            {
                QuestionId = questionId,
                AuditionId = auditionId,
                TopicId = topicId,
                TestId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Description = Guid.NewGuid().ToString()
            };

            //Act
            var validator = new CreateReportCommand.Validator();
            var validationResult = validator.Validate(request);

            //Assert
            Assert.NotNull(validationResult);
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Errors);
        }
    }
}