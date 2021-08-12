using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace Elevel.Infrastructure.Services.Implementation
{
    public class FileService : IFileService
    {
        private readonly string folderPath = Path.Combine("wwwroot", "files");

        public async Task<string> UploadFiles(List<IFormFile> files)
        {
            if (files.Count > 1)
            {
                throw new ValidationException("There should be only one file to upload at a time!");
            }

            if (!File.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var file = files[0];
            if (file.Length <= 0)
            {
                return string.Empty;
            }
            var fileExtension = Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, $"file{DateTime.Now:yyyy_MM_dd-HH_mm_ss}{fileExtension}");
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return filePath;
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