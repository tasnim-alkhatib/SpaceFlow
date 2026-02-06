using Microsoft.AspNetCore.Hosting;
using SpaceFlow.Core.IRepository;
using SpaceFlow.Core.IServices;

namespace SpaceFlow.Infrastructure.FileStorage
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment) => _webHostEnvironment = webHostEnvironment;

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folderName)
        {
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", folderName);
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            using var localStream = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(localStream);

            return uniqueFileName;
        }

        public void DeleteFile(string fileName, string folderName)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", folderName, fileName);
            if (File.Exists(filePath)) File.Delete(filePath);
        }
    }
}
