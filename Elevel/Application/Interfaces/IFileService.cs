using Elevel.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elevel.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFiles(List<IFormFile> files);
        FileType DownloadFile(string path);
    }
}