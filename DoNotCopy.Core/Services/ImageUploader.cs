using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DoNotCopy.Core.Services
{
    public class LocalImageUploader : IImageUploader
    {
        public async Task<string> UploadAsync(IFormFile file)
        {
            if (file.Length > 0)
            {
                try
                {
                    if (!Directory.Exists("actualpath"))
                    {
                        Directory.CreateDirectory("actualpath");
                    }
                    using (FileStream filestream = System.IO.File.Create("actualpath" + file.FileName))
                    {
                        await file.CopyToAsync(filestream);
                        filestream.Flush();
                        return file.FileName;
                    }
                }
                catch (System.Exception ex)
                {
                    return ex.ToString();
                }
            }
            else
            {
                return "Unsuccessful";
            }

        }

     
    }
}
