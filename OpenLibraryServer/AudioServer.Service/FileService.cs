using AudioServer.DataAccess;
using AudioServer.Models;
using AudioServer.Models.DTOs;
using AudioServer.Service.Exceptions;
using AudioServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AudioServer.Services
{
    public class FileService : IFileService
    {
        private readonly AudioServerDBContext _dbContext;
        private readonly IFileServerClient _fileServerClient;
        public FileService(AudioServerDBContext dbContext, IFileServerClient client)
        {
            _dbContext = dbContext;
            _fileServerClient = client;
        }

        public async Task<FileEntity> AddFile(CreateEditFileTO fileToCreate)
        {


            await _dbContext.Files.AddAsync(newFile);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<FileEntity> EditFile(CreateEditFileTO newFile)
        {
            FileEntity result = await _dbContext.Files.FirstOrDefaultAsync(f => f.FileId == newFile.FileId);
            if (result != null)
            {
                bool updateNeeded = false;

                // first check if any field changed.
                if (result.Name != newFile.Name 
                    || result.Author != newFile.Author
                    || result.Description != result.Description) 
                {
                    result.Name = newFile.Name;
                    result.Author = newFile.Author;
                    result.Description = newFile.Description;

                    updateNeeded = true;
                }

                // check that content changed -> update.
                string currentFileContent = await _fileServerClient.ReadFile(result.Name);
                if (currentFileContent != newFile.Base64File) 
                {
                    string fileURL = await _fileServerClient.AddFile(newFile.Base64File, newFile.Name);
                    
                    result.FileURL = fileURL;

                    updateNeeded = true;
                }

                if (updateNeeded)
                {
                    _dbContext.Files.Update(result);
                    await _dbContext.SaveChangesAsync();
                }
            }
            else
                throw new NotFoundException($"File with Id {newFile.FileId} not found");

            return result;
        }

        public async Task<IEnumerable<FileEntity>> GetAll()
        {
            List<FileEntity> allFiles = await _dbContext.Files.ToListAsync();

            return allFiles;
        }
    }
}
