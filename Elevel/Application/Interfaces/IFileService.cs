using Elevel.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elevel.Application.Interfaces
{
    public interface IFileService
    {
        void UploadFiles(List<IFormFile> files);
        FileZip DownloadFiles();
        string SizeConverter(long bytes);
    }
}