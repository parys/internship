using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Elevel.Domain.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public DateTimeOffset CreationDate { get; set; }

        public ICollection<Test> UserTests { get; set; }
        public ICollection<Test> HrTests { get; set; }
        public ICollection<Test> CoachTests { get; set; }
        public ICollection<Report> UserReports { get; set; }
        public ICollection<Report> CreatorReports { get; set; }
        public ICollection<Audition> Auditions { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<Topic> Topics { get; set; }
        
    }
}