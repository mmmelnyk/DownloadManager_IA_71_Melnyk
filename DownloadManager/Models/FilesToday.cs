using System;
using System.Collections.Generic;
using System.Linq;

namespace DownloadManager.Models
{
    class FilesToday : DataAccess
    {
        public override void Select()
        {
            var today = DateTime.Now.ToString("M/d/yyyy");
            var allFiles = filesContext.Files.ToList();
            foreach (var file in allFiles)
            {
                if(file.DateTime.ToString("M/d/yyyy")==today)
                    filesList.Add(file);
            }
        }

        public override List<File> Process()
        {
            return filesList;
        }
    }
}
