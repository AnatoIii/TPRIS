using AudioServer.FileServer.DTOs;
using AudioServer.FileServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace AudioServer.FileServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "pong";
        }

        [HttpPost]
        public IActionResult AddFile([FromBody] SaveFileRequest request)
        {
            string filePath = _fileService.AddFile(request.FileContent, request.RelativePath);

            return Ok(filePath);
        }

        [HttpGet]
        public IActionResult GetFileContentByPath([FromQuery] string path)
        {
            string fileContent = _fileService.GetFileContentByPath(path);

            return Ok(fileContent);
        }
    }
}
