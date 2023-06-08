using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DoNotCopy.Core.Services
{
    public interface IImageUploader
    {
       Task<string> UploadAsync(IFormFile file);
    }

}
