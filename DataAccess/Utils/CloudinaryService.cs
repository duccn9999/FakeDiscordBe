using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json.Linq;

namespace DataAccesses.Utils
{
    public class CloudinaryService
    {
        private const string API_SECRET_KEY = "vA2g3XJb6DfuY8D5MeZyL4GEybQ";
        private Cloudinary cloudinary;
        public CloudinaryService()
        {
            cloudinary = new Cloudinary($"cloudinary://531895637933677:{API_SECRET_KEY}@dywexvvcy");
            cloudinary.Api.Secure = true;
        }
        public JToken Upload(string imgBase64)
        {
            // Ensure the Base64 string is prefixed correctly
            if (!imgBase64.StartsWith("data:image"))
            {
                imgBase64 = $"data:image/png;base64,{imgBase64}"; // Default to PNG if no prefix
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imgBase64), // Pass the Base64 string
                AssetFolder = "images" // Specify the asset folder
            };

            var result = cloudinary.Upload(uploadParams);

            return result.JsonObj; // Return the JSON response
        }

        public JToken Delete(string img)
        {
            var deleteParams = new DelResParams()
            {
                PublicIds = new List<string> { $"images/{img}" },
                Type = "upload",
                ResourceType = ResourceType.Image
            };
            var result = cloudinary.DeleteResources(deleteParams);
            return result.JsonObj;
        }
        public JToken Modify(string oldName, string newName)
        {
            var renameParams = new RenameParams($"images/{oldName}", $"images/{newName}")
            {
                Type = "upload",
                ResourceType = ResourceType.Image
            };
            var result = cloudinary.Rename(renameParams);
            return result.JsonObj;
        }
    }
}
