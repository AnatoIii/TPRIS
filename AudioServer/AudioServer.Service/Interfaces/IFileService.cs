using AudioServer.Models;
using AudioServer.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AudioServer.Services.Interfaces
{
    public interface IFileService
    {
        Task<FileEntity> AddFile(CreateEditFileTO fileToCreate);

        Task<FileEntity> EditFile(CreateEditFileTO newFile);

        Task<IEnumerable<FileEntity>> GetAll();
    }
}
