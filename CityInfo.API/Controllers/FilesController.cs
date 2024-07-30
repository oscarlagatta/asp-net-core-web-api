using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [Route("api/v{version:apiVersion}/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ??
                                                throw new System.ArgumentNullException(
                                                    nameof(fileExtensionContentTypeProvider));
        }

        /// <summary>
        /// Retrieves a file based on the given fileId.
        /// </summary>
        /// <param name="fileId">The ID of the file to retrieve.</param>
        /// <returns>An ActionResult object representing the file.</returns>
        [HttpGet("{fileId}")]
        [ApiVersion(0.1, Deprecated = true)]
        public ActionResult GetFile(string fileId)
        {
            // look up the actual file, depending on the fieldId...
            // demo code
            var pathToFile = "Oscar Lagatta-06-2024.pdf";

            // check whether the file exists
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            if (!_fileExtensionContentTypeProvider.TryGetContentType(
                    pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            
            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType , Path.GetFileName(pathToFile));
        }

        /// <summary>
        /// Creates a file based on the given input file.
        /// </summary>
        /// <param name="file">The input file to create.</param>
        /// <returns>An ActionResult representing the result of the file creation.</returns>
        [HttpPost]
        public async Task<ActionResult> CreateFile(IFormFile file)
        {
            // Validate the input. Put a limit on filesize to avoid large uploads attacks.
            // Only accept .pdf files (check content-type)
            if (file.Length == 0 || file.Length > 20971520 || file.ContentType != "application/json")
            {
                return BadRequest("No file or an invalid on has been inputted");
            }
            
            // Create a file path. Avoid using file.FileName, as an attacker can provide a 
            // malicious one, including full paths or relative paths.
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                $"uploaded_file_{Guid.NewGuid()}.pdf"
            );

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok();
        }
    }
}