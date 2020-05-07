using System.Data.Entity;

namespace DownloadManager.Models
{
    public class FilesContext : DbContext
    {
        public DbSet<File> Files { get; set; }
    }
}
