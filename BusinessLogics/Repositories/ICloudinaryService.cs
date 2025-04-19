using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BusinessLogics.Repositories
{
    public interface ICloudinaryService
    {
        public Task<string> UploadImage(IFormFile img);
        public Task UpdateImage(string img);
        public Task<DeletionResult> DeleteImage(string publicId);
        public string GetImagePublicId(string img);
    }
}
