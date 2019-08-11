using Microsoft.EntityFrameworkCore;
using QuickAdmin.Model.Entities;

namespace QuickAdmin.DAO
{
    public class QuickAdminDbContext: DbContext
    {

        public QuickAdminDbContext(DbContextOptions<QuickAdminDbContext> options)
            : base(options)
        {
        }

        public int TenantId { get; set; }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<AuditLog> AuditLog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(p => !p.IsDeleted); //&& p.TenantId == this.TenantId
        }
    }
}
