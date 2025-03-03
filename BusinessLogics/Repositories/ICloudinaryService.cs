using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.Repositories
{
    public interface ICloudinaryService
    {
        public Task<string> UploadImage(string img);
        public Task UpdateImage(string img);
        public Task DeleteImage(string img);
        public string GetImagePublicId(string img);
    }
}
