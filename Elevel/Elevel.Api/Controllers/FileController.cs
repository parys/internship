using Elevel.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Elevel.Api.Controllers
{
    [Route("api/[controller]"), Authorize]
    [ApiController]
    public class FileController : BaseApiController
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
        /// Downloads a file from @"wwwroot\files" folder according to its path.
        /// Receives the name of the file we need to download from @"wwwroot\files".
        /// Returns the file with content according or error if this file doesn't exist.
        /// </summary>
        /// <param name="filePath">The path of file</param>
        /// <returns></returns>
        [HttpGet(nameof(Download))]
        public IActionResult Download([Required] string filePath)
        {
            var file =  _fileService.DownloadFile(filePath);
            return File(file.Content, file.ContentType, file.FileName);
        }
    }
}