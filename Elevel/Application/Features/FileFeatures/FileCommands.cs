using Elevel.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Elevel.Application.Features.FileFeatures
{
    public class FileCommands : IFileService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileCommands(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void UploadFile(List<IFormFile> files, string subDirectory)
        {
            subDirectory ??= string.Empty;
            var target = Path.Combine(_hostingEnvironment.ContentRootPath, subDirectory);

            Directory.CreateDirectory(target);

            files.ForEach(async file =>
            {
                if (file.Length <= 0) return;
                var filePath = Path.Combine(target, file.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
            });
        }

        public (string fileType, byte[] archiveData, string archiveName) DownloadFiles(string subDirectory)
        {
            var zipName = $"archive-{DateTime.Now:yyyy_MM_dd-HH_mm_ss}.zip";

            var files = Directory.GetFiles(Path.Combine(_hostingEnvironment.ContentRootPath, subDirectory)).ToList();

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

            return ("application/zip", memoryStream.ToArray(), zipName);

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