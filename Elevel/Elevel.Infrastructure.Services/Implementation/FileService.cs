using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace Elevel.Infrastructure.Services.Implementation
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void UploadFiles(List<IFormFile> files)
        {
            var target = @"wwwroot\files";
            if (!File.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            files.ForEach(async file =>
            {
                if (file.Length <= 0) return;
                var fileExtension = Path.GetExtension(file.FileName);
                var filePath = Path.Combine(target, $"myFile{DateTime.Now:yyyy_MM_dd-HH_mm_ss}{fileExtension}");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            });
        }

        public FileType DownloadFile(string fileName)
        {
            var net = new System.Net.WebClient();
            var data = net.DownloadData(Path.Combine(@"wwwroot\files", fileName));
            var content = new System.IO.MemoryStream(data);
            var contentType = "APPLICATION/octet-stream";
            return new FileType
            {
                Content = content,
                ContentType = contentType,
                FileName = fileName
            };
        }
    }
}