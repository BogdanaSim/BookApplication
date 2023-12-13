using BookApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("BooksDB"));
        }

        public DbSet<Book> Books { get; set; }
    }
}