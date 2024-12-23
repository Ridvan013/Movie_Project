using Microsoft.EntityFrameworkCore;

namespace Movie_Project.Models
{
    public class MovieDbContext : DbContext
    {

        public MovieDbContext()
        {
            
        }
        // Constructor
        public MovieDbContext(DbContextOptions<MovieDbContext> options): base(options)
        {
        }

        // DbSet'ler - Veritabanı Tablolarını Tanımlar
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Review> Reviews { get; set; }

        // Model Oluşturma (Fluent API Kullanarak İlişkiler Tanımlama)
        
    }
}
