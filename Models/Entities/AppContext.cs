using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Musala.Models
{
    public partial class MusalaTestContext : DbContext
    {
        public MusalaTestContext()
        {
        }

        public MusalaTestContext(DbContextOptions<MusalaTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Gateway> Gateway { get; set; }
        public virtual DbSet<Peripheral> Peripheral { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gateway>(entity =>
            {
                entity.ToTable("gateway");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Ipv4)
                    .HasColumnName("ipv4")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime");

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<Peripheral>(entity =>
            {
                entity.ToTable("peripheral");

                entity.HasIndex(e => e.GatewayId)
                    .HasName("Gateway_Peripheral");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreation)
                    .HasColumnName("date_creation")
                    .HasColumnType("datetime");

                entity.Property(e => e.GatewayId)
                    .HasColumnName("gateway_id")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Status).HasColumnName("status");                

                entity.Property(e => e.Vendor)
                    .HasColumnName("vendor")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Gateway)
                    .WithMany(p => p.Peripherals)
                    .HasForeignKey(d => d.GatewayId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Gateway_Peripheral");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.HasQueryFilter(m => !m.IsDeleted);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime");

                
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        entry.CurrentValues["CreatedAt"] = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
            }
        }
    }
}
