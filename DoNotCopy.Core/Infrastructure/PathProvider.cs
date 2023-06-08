using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.IO;

namespace DoNotCopy.Core.Infrastructure
{
    public class PathProvider : IPathProvider
    {
        private readonly GeneralSettings _generalSettings;

        public PathProvider(
            IOptions<GeneralSettings> generalSettings)
        {
            _generalSettings = generalSettings.Value;
        }

        public string MapPath(string path)
        {
            string filePath = Path.Combine(_generalSettings.FileStoragePath, path);
            return filePath;
        }

        public string MapPath(string path1, string path2)
        {
            string filePath = Path.Combine(_generalSettings.FileStoragePath, path1, path2);
            return filePath;
        }

        public string MapPath(string path1, string path2, string path3)
        {
            string filePath = Path.Combine(_generalSettings.FileStoragePath, path1, path2, path3);
            return filePath;
        }

        public string MapPath(string path1, string path2, string path3,string path4)
        {
            string filePath = Path.Combine(_generalSettings.FileStoragePath, path1, path2, path3, path4);
            return filePath;
        }
    }
}
