using Microsoft.AspNetCore.Http;

namespace Infrastructure.Interfaces
{
    public interface IFileManager
    {
        Task<string> SaveFile(IFormFile file, string? directory = null);
        void DeleteFile(string path);
    }
}
