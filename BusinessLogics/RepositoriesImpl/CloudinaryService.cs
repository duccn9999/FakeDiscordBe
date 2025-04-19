using BusinessLogics.Repositories;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;

namespace BusinessLogics.RepositoriesImpl
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;
        const string CLOUDINARY_CONNECT_STRING = "cloudinary://531895637933677:vA2g3XJb6DfuY8D5MeZyL4GEybQ@dywexvvcy";
        public CloudinaryService()
        {
            cloudinary = new Cloudinary(CLOUDINARY_CONNECT_STRING);
            cloudinary.Api.Secure = true;
        }

        public async Task<DeletionResult> DeleteImage(string publicId)
        {
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
            };
            var result = await cloudinary.DestroyAsync(deleteParams);
            return result;
        }

        public async Task UpdateImage(string img)
        {
            var updateParams = new UpdateParams(img)
            {
                Tags = "important",
                ModerationStatus = ModerationStatus.Approved
            };
            var updateResult = cloudinary.UpdateResource(updateParams);
        }

        public async Task<string> UploadImage(IFormFile img)
        {
            var uploadResult = new ImageUploadResult();
            if (img.Length > 0)
            {
                using var stream = img.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(img.FileName, stream),
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
            else
            {
                throw new Exception("Image is empty");
            }
        }

        public string GetImagePublicId(string img)
        {
            string lastPart = img.Substring(img.LastIndexOf('/') + 1);
            string filenameWithoutExtension = lastPart.Substring(0, lastPart.LastIndexOf('.'));
            return filenameWithoutExtension;
        }
    }
}
