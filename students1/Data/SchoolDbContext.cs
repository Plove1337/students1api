using Microsoft.EntityFrameworkCore;
using students1.Models;

namespace students1.Data
{
    public class SchoolDbContext : DbContext
    { 
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }

        public SchoolDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>()
                .HasMany(c => c.Students)
                .WithOne(s => s.Class)
                .HasForeignKey(s => s.ClassID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
