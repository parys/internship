using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace Elevel.Infrastructure.Services.Implementation
{
    public class FileService : IFileService
    {
        private readonly string folderPath = Path.Combine("wwwroot", "files");

        public void UploadFiles(List<IFormFile> files)
        {
            if (!File.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            files.ForEach(async file =>
            {
                if (file.Length <= 0) return;
                var fileExtension = Path.GetExtension(file.FileName);
                var filePath = Path.Combine(folderPath, $"myFile{DateTime.Now:yyyy_MM_dd-HH_mm_ss}{fileExtension}");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            });
        }

        public FileType DownloadFile(string fileName)
        {
            var file = File.ReadAllBytes(Path.Combine(folderPath, fileName));
            var content = new MemoryStream(file);
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