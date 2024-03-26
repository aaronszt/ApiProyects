using ApiProyects.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProyects.Files
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    Name = "Test",
                    Email = "Test",
                    Password = "Test",
                    City = "Test",
                    Country = "Test",
                    ImageUrl = "Test",
                    CreationDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                });
        }
    }
}
