using System.Threading.Tasks;

namespace AudioServer.Services.Interfaces
{
    public interface IFileServerClient
    {
        Task<string> AddFile(string fileContent, string relativePath);

        Task<string> ReadFile(string relativePath);
    }
}
