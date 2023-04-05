using Application.Interfaces.FileManager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Application.Features.FileManager
{
    public class FileManager : IFileManager
    {
        private readonly IWebHostEnvironment env;
        private readonly string uploadFolder = "/uploads";

        public FileManager(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public void DeleteFile(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            var fullPath = $"{env.WebRootPath}{uploadFolder}/{path}";
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public async Task<string> SaveFile(IFormFile file, string? directory = null)
        {
            var path = Path.Combine(uploadFolder, $"{directory}/{DateTime.Now.Ticks}_{file.Name}");
            using var stream = new FileStream(Path.Combine(env.WebRootPath, path), FileMode.Create);
            await file.CopyToAsync(stream);
            return path;
        }
    }
}
