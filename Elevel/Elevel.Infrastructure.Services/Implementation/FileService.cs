using Elevel.Application.Infrastructure;
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
        public async Task<string> UploadFiles(List<IFormFile> files)
        {
            if (files.Count > 1)
            {
                throw new ValidationException("There should be only one file to upload at a time!");
            }

            if (!File.Exists(Constants.FILE_FOLDER_PATH))
            {
                Directory.CreateDirectory(Constants.FILE_FOLDER_PATH);
            }

            var file = files[0];
            if (file.Length <= 0)
            {
                return string.Empty;
            }
            var fileExtension = Path.GetExtension(file.FileName);
            var filePath = $"file{DateTime.Now:yyyy_MM_dd-HH_mm_ss}{fileExtension}";
            using var stream = new FileStream(Path.Combine(Constants.FILE_FOLDER_PATH,filePath), FileMode.Create);
            await file.CopyToAsync(stream);

            return filePath;
        }

        public FileType DownloadFile(string fileName)
        {
            if (fileName != null && !File.Exists(Path.Combine(Constants.FILE_FOLDER_PATH, fileName)))
            {
                throw new NotFoundException($"File with path {fileName} not found.");
            }

            return new FileType
            {
                Stream = new(Path.Combine(Constants.FILE_FOLDER_PATH, fileName), FileMode.Open, FileAccess.Read),
                ContentType = "application/octet-stream",
                FileName = fileName
            };
        }
    }
}