using System.Collections.Generic;

namespace Elevel.Domain.Models
{
    public class Role : BaseDataModel
    {
        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}