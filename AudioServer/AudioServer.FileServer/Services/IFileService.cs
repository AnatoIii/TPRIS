namespace AudioServer.FileServer.Services
{
    public interface IFileService
    {
        string AddFile(string fileContentToSave, string fileRelativePath);
        string GetFileContentByPath(string path);
    }
}
