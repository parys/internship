using System.IO;

namespace Elevel.Domain.Models
{
    public class FileType
    {
        public MemoryStream Content { get; set; }
        public string ContentType {get; set;}
        public string FileName {get; set;}
    }
}
