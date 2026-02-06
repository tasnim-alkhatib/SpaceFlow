
namespace SpaceFlow.Core.IServices
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string folderName); void DeleteFile(string fileName, string folderName);
    }
}
