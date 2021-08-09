using Elevel.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Elevel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Uploads the files from the list.
        /// Receives the list of the files we need to transport to @"wwwroot\files".
        /// Returns nothing.
        /// </summary>
        /// <param name="formFiles">Files (as a list)</param>
        /// <returns></returns>
        [HttpPost(nameof(Upload))]
        public IActionResult Upload([Required] List<IFormFile> formFiles)
        {
            _fileService.UploadFiles(formFiles);
            return Ok();
        }

        /// <summary>
        /// Downloads all files from folder @"wwwroot\files" as a zip-archive.
        /// Receives nothing - everything is completed inside the function.
        /// Returns the ZIP-file with all the files. 
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(Download))]
        public IActionResult Download()
        {
             var Zip = _fileService.DownloadFiles(@"wwwroot\files");
             return File(Zip.ArchiveData, Zip.FileType, Zip.ArchiveName);
        }
    }
}