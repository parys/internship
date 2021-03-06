using System;
using System.Collections.Generic;
using Elevel.Domain.Enums;

namespace Elevel.Domain.Models
{
    public class Audition : BaseDataModel
    {
        public long AuditionNumber { get; set; }
        public string AudioFilePath { get; set; }
        public Level Level { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public bool Deleted { get; set; } = false;

        public Guid CreatorId { get; set; }
        public User Creator { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<Test> Tests { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}