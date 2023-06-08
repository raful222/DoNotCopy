using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DoNotCopy.Api.Services
{
    public class ClearFolderService
    {
        public ClearFolderService()
        {
        }
        public void ClearFolder(string FolderName)
        {
            DirectoryInfo folder = new DirectoryInfo(FolderName);

            foreach (FileInfo file in folder.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in folder.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
