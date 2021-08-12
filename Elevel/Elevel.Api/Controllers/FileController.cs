using Elevel.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

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
        /// Uploads a single file from the list of files.
        /// Receives the list of the files.
        /// Returns a path to the uploaded file or error if there were 2 or more files in the list.
        /// </summary>
        /// <param name="formFiles">Files (as a list)</param>
        /// <returns></returns>
        [HttpPost(nameof(Upload))]
        public async Task<IActionResult> Upload([Required] List<IFormFile> formFiles)
        {
            var pathFile = await _fileService.UploadFiles(formFiles);
            return Ok(new { pathfile = pathFile });
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
            var file = _fileService.DownloadFile(filePath);
            return File(file.Content, file.ContentType, file.FileName);
        }
    }
}