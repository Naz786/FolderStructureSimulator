using System.Data.Entity;

using static MiniWebProject.Models.MWPModels;

namespace MiniWebProject.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }
        
        public DbSet<Bookmark> Bookmarks { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}