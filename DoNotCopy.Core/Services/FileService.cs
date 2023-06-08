using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using DoNotCopy.Core.Infrastructure;
using DoNotCopy.Core.Data;
using File = DoNotCopy.Core.Entities.File;
using DoNotCopy.Core.Entities;

namespace DoNotCopy.Core.Services
{
    public class FileService
    {
        private readonly IPathProvider _pathProvider;
        private readonly ILogger<FileService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly GeneralSettings _generalSettings;

        public FileService(
            IPathProvider pathProvider,
            ILogger<FileService> logger,
            ApplicationDbContext context,
            IOptions<GeneralSettings> generalSettings)
        {
            _pathProvider = pathProvider;
            _logger = logger;
            _context = context;
            _generalSettings = generalSettings.Value;
        }
        public async Task<IFormFile> CastFilePathToIFormFile(string filePath)
        {
            string fileName = Path.GetFileName(filePath);

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                var fileBytes = memoryStream.ToArray();
                var file = new FormFile(memoryStream, 0, fileBytes.Length, fileName, fileName);
                return file;
            }
        }



        public async Task<File> CreateFileAsync(IFormFile fileUpload, FileType fileType ,string locPath)
        {
            string fileName = Path.GetFileName(fileUpload.FileName);
            string fileExtension = Path.GetExtension(fileUpload.FileName);

            var file = new File
            {
                Name = Guid.NewGuid().ToString() + fileExtension,
                Type = fileType
            };

            var path = _pathProvider.MapPath(locPath, file.Name);

            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await fileUpload.CopyToAsync(stream);
            }

            _context.Files.Add(file);

            return file;
        }

        public async Task<File> UpdateFileAsync(IFormFile fileUpload, File file)
        {
            string fileName = Path.GetFileName(fileUpload.FileName);
            string fileExtension = Path.GetExtension(fileUpload.FileName);

            file.Name = Guid.NewGuid().ToString() + fileExtension;

            var path = _pathProvider.MapPath(_generalSettings.UploadPath, fileName);

            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await fileUpload.CopyToAsync(stream);
            }

            return file;
        }

        public void DeleteFile(string fileName)
        {
            // Delete file from the file system
            var path = _pathProvider.MapPath(_generalSettings.UploadPath, fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}
