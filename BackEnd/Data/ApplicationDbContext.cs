using Microsoft.EntityFrameworkCore;
using BackEnd.Data;

namespace BackEnd.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }

        public DbSet<BackEnd.Data.Questions> Questions { get; set; }

        public DbSet<BackEnd.Data.Comments> Comments { get; set; }
    }
}
