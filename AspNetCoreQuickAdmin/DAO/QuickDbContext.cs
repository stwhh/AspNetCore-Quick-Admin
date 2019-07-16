using System;
using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace DAO
{
    public class QuickDbContext: DbContext
    {
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
