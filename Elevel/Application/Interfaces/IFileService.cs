using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elevel.Application.Interfaces
{
    public interface IFileService
    {
        void UploadFile(List<IFormFile> files, string subDirectory);
        (string fileType, byte[] archiveData, string archiveName) DownloadFiles(string subDirectory);
        string SizeConverter(long bytes);
    }
}