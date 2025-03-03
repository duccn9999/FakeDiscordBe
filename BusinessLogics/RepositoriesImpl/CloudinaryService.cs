using BusinessLogics.Repositories;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using dotenv.net;
using Newtonsoft.Json.Linq;
using System;

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

        public async Task DeleteImage(string img)
        {
            throw new NotImplementedException();
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

        public async Task<string> UploadImage(string img)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(@$"{img}"),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }

        public string GetImagePublicId(string img)
        {
            string lastPart = img.Substring(img.LastIndexOf('/') + 1);
            string filenameWithoutExtension = lastPart.Substring(0, lastPart.LastIndexOf('.'));
            return filenameWithoutExtension;
        }
    }
}
