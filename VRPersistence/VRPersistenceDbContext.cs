using Microsoft.EntityFrameworkCore;
using VRPersistence.DAO;

namespace VRPersistence
{
    public class VRPersistenceDbContext : DbContext
    {
        public VRPersistenceDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Release> Releases { get; set; }
        public DbSet<Media> Media { get; set; }
    }
}