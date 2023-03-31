using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.FileManager
{
    public interface IFileManager
    {
        Task<string> SaveFile(IFormFile file, string? directory = null);
        void DeleteFile(string path);
    }
}
