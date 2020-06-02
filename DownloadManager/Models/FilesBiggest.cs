using System;
using System.Collections.Generic;
using System.Linq;

namespace DownloadManager.Models
{
    class FilesBiggest : DataAccess
    {
        public override void Select()
        {
            var allFiles = filesContext.Files.ToList();
            foreach (var file in allFiles)
            {
                if (file.FileSize != null)
                {
                    string tmp = "";
                    tmp = file.FileSize.Split(' ').ToList().First();
                    int size = Int32.Parse(tmp);
                    if (size > 100)
                        filesList.Add(file);
                }
            }
        }

        public override List<File> Process()
        {
            return filesList;
        }
    }
}
