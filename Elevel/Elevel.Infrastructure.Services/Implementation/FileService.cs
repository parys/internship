using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

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
            Directory.CreateDirectory(target);
            files.ForEach(async file =>
            {
                if (file.Length <= 0) return;
                var filePath = "example.txt";
                for (int i = 0; ; ++i)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    filePath = Path.Combine(target, fileName + (i > 0 ? $" ({i})" : "") + fileExtension);
                    if (!File.Exists(filePath)) break;
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            });
        }

        public FileZip DownloadFiles(string fromDirectory)
        {
            var zipName = $"archive-{DateTime.Now:yyyy_MM_dd-HH_mm_ss}.zip";

            if (!File.Exists(fromDirectory))
            {
                Directory.CreateDirectory(fromDirectory);
            }
            var files = Directory.GetFiles(fromDirectory).ToList();

            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                files.ForEach(file =>
                {
                    var theFile = archive.CreateEntry(file);
                    using var streamWriter = new StreamWriter(theFile.Open());
                    streamWriter.Write(File.ReadAllText(file));
                });
            }

            return new FileZip
            {
                FileType = "application/zip",
                ArchiveData = memoryStream.ToArray(),
                ArchiveName = zipName
            };

        }
        public string SizeConverter(long bytes)
        {
            var fileSize = new decimal(bytes);
            var kilobyte = new decimal(1024);
            var megabyte = new decimal(1024 * 1024);
            var gigabyte = new decimal(1024 * 1024 * 1024);

            return fileSize switch
            {
                var _ when fileSize < kilobyte => $"Less then 1KB",
                var _ when fileSize < megabyte => $"{Math.Round(fileSize / kilobyte, 0, MidpointRounding.AwayFromZero):##,###.##}KB",
                var _ when fileSize < gigabyte => $"{Math.Round(fileSize / megabyte, 2, MidpointRounding.AwayFromZero):##,###.##}MB",
                var _ when fileSize >= gigabyte => $"{Math.Round(fileSize / gigabyte, 2, MidpointRounding.AwayFromZero):##,###.##}GB",
                _ => "n/a",
            };
        }

    }
}