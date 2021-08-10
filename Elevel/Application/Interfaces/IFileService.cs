using Elevel.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Elevel.Application.Interfaces
{
    public interface IFileService
    {
        void UploadFiles(List<IFormFile> files);
        FileType DownloadFile(string path);
    }
}