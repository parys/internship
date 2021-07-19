using System;
using System.Collections.Generic;
using Elevel.Domain.Enums;

namespace Elevel.Domain.Models
{
    public class Audition : BaseDataModel
    {
        public string AudioFilePath { get; set; }
        public Level Level { get; set; }
        public DateTimeOffset CreationDate { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<Test> Tests { get; set; }
    }
}