using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using System;

namespace BackEnd.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }

        public DbSet<Questions> Questions { get; set; }

        public DbSet<Comments> Comments { get; set; }

        public DbSet<Answers> Answers { get; set; }        
    }
}
