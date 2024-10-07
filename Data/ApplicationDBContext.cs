using Microsoft.EntityFrameworkCore;
using KwFeeds.Models;

namespace KwFeeds.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HomePageBanner> HomePageBanners { get; set; }
    }
}