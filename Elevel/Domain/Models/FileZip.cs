using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Domain.Models
{
    public class FileZip
    {
        public string FileType { get; set; }
        public byte[] ArchiveData { get; set; }
        public string ArchiveName { get; set; }
    }
}
