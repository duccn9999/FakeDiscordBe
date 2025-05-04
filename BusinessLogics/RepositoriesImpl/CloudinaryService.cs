using BusinessLogics.Repositories;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;

namespace BusinessLogics.RepositoriesImpl
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;
        const string API_KEY = "vA2g3XJb6DfuY8D5MeZyL4GEybQ";
        const string CLOUDINARY_CONNECT_STRING = $"cloudinary://531895637933677:{API_KEY}@dywexvvcy";
        public CloudinaryService()
        {
            cloudinary = new Cloudinary(CLOUDINARY_CONNECT_STRING);
            cloudinary.Api.Secure = true;
        }

        public async Task<DeletionResult> DeleteAttachment(string publicId)
        {
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Auto
            };
            var result = await cloudinary.DestroyAsync(deleteParams);
            return result;
        }

        public async Task UpdateAttachment(string file)
        {
            var updateParams = new UpdateParams(file)
            {
                Tags = "important",
                ModerationStatus = ModerationStatus.Approved
            };
            var updateResult = cloudinary.UpdateResource(updateParams);
        }

        public async Task<CloundinaryResponse> UploadAttachment(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new AutoUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true,
                    Transformation = new Transformation().Quality("auto").FetchFormat("auto").Crop("scale"),
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
                return new CloundinaryResponse
                {
                    AssetId = uploadResult.AssetId,
                    PublicId = uploadResult.PublicId,
                    Width = uploadResult.Width.ToString(),
                    Height = uploadResult.Height.ToString(),
                    Format = uploadResult.Format,
                    ResourceType = uploadResult.ResourceType,
                    CreatedAt = uploadResult.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                    Pages = uploadResult.Pages,
                    Bytes = uploadResult.Bytes.ToString(),
                    Type = uploadResult.Type,
                    Placeholder = uploadResult.Placeholder.ToString(),
                    Url = uploadResult.Url.ToString(),
                    SecureUrl = uploadResult.SecureUrl.ToString(),
                    AssetFolder = uploadResult.AssetFolder,
                    DisplayName = uploadResult.DisplayName,
                    OriginalFilename = uploadResult.OriginalFilename,
                };
            }
            else
            {
                throw new Exception("File is empty");
            }
        }

        public string GetDownloadLink(string url, string originalFileName, string displayName)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            const string uploadSegment = "/upload/";
            int index = url.IndexOf(uploadSegment);
            if (index == -1)
                return url;

            // Build the transformation string
            string transformation = $"f_auto/fl_attachment:{originalFileName}/";

            // Insert transformation right after /upload/
            string result = url.Insert(index + uploadSegment.Length, transformation);

            return result;
        }

    }
}
