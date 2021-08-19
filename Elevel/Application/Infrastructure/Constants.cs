using System.IO;

namespace Elevel.Application.Infrastructure
{
    public static class Constants
    {
        public const int AUDTUION_MIN_AMOUNT = 1;
        public const int TOPIC_MIN_AMOUNT = 2;
        public const int MIN_MARK = 0;
        public const int MAX_MARK = 10;
        public const int ESSAY_MAX_LENGTH = 512;
        public const int TEST_DURATION = 61; //minutes
        public const int LEVEL_AMOUNT = 5;
        public const int GRAMMAR_QUESTION_AMOUNT = 12;
        public const int AUDITION_QUESTION_AMOUNT = 10;
        public const int ANSWER_AMOUNT = 4;
        public const int CORRECT_ANSWER_AMOUNT = 1;
        public const int ANSWERS_AMOUNT_PER_QUESTION = 1;
        public static readonly string FILE_FOLDER_PATH = Path.Combine("wwwroot", "files");
        public static readonly string EMAIL_PATH = Path.Combine("wwwroot", "email");
        public static readonly string EMAIL_TEMPLATE = @"EmailTemplate.html";
    }
}
