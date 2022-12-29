using AudioServer.Models;
using AudioServer.Models.DTOs;
using AudioServer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AudioServer.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly IFileService _fileService;

        public BooksController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IEnumerable<FileEntity>> GetAll()
        {
            IEnumerable<FileEntity> result = await _fileService.GetAll();

            return result;
        }

        [HttpPost]
        public async Task<FileEntity> EditFile([FromBody] CreateEditFileTO newFile)
        {
            FileEntity result = await _fileService.EditFile(newFile);

            return result;
        }

        [HttpPost]
        public async Task<FileEntity> CreateFile([FromBody] CreateEditFileTO fileToCreate)
        {
            FileEntity newFile = await _fileService.AddFile(fileToCreate);

            return newFile;
        }
    }
}