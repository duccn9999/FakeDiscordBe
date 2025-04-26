using BusinessLogics.RepositoriesImpl;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BusinessLogics.Repositories
{
    public class CloundinaryResponse
    {
        public string AssetId { get; set; }
        public string PublicId { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Format { get; set; }
        public string ResourceType { get; set; }
        public string CreatedAt { get; set; }
        public int Pages { get; set; }
        public string Bytes { get; set; }
        public string Type { get; set; }
        public string Placeholder { get; set; }
        public string Url { get; set; }
        public string SecureUrl { get; set; }
        public string AssetFolder { get; set; }
        public string DisplayName { get; set; }
        public string OriginalFilename { get; set; }
    }
    public interface ICloudinaryService
    {
        public Task<CloundinaryResponse> UploadAttachment(IFormFile file);
        public Task UpdateAttachment(string file);
        public Task<DeletionResult> DeleteAttachment(string publicId);
        public string GetDownloadLink(string url, string origialFileName, string displayName);
    }
}
