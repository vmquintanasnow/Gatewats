using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Musala.Domain.Entity;

namespace Musala.DAL
{
    public partial class MusalaContext : DbContext
    {
        public MusalaContext()
        {
        }

        public MusalaContext(DbContextOptions<MusalaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Gateway> Gateways { get; set; }
        public virtual DbSet<Device> Devices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Gateway>(entity =>
            {                
                entity.ToTable("gateway");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.Ipv4)
                    .HasColumnName("ipv4")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime");

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("device");

                entity.HasIndex(e => e.GatewayId)
                    .HasName("Gateway_Device");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreation)
                    .HasColumnName("date_creation")
                    .HasColumnType("datetime");

                entity.Property(e => e.GatewayId)
                    .HasColumnName("gateway_id")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Vendor)
                    .HasColumnName("vendor")
                    .HasColumnType("varchar(64)");

                entity.HasOne(d => d.Gateway)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.GatewayId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Gateway_Device");

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

                        foreach (var navigationEntry in entry.Navigations.Where(n => n.Metadata.IsDependentToPrincipal()))
                        {
                            if (navigationEntry is CollectionEntry collectionEntry)
                            {
                                foreach (var dependentEntry in collectionEntry.CurrentValue)
                                {
                                    HandleDependent(Entry(dependentEntry));
                                }
                            }
                            else
                            {
                                var dependentEntry = navigationEntry.CurrentValue;
                                if (dependentEntry != null)
                                {
                                    HandleDependent(Entry(dependentEntry));
                                }
                            }
                        }                        
                        break;
                }
            }
        }
        private void HandleDependent(EntityEntry entry)
        {
            entry.CurrentValues["IsDeleted"] = true;
        }
    }
}
