using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PermissionsN5.Core.Entities;

namespace PermissionsN5.Infraestructure.Data
{
    public partial class N5SolucionContext : DbContext
    {
        public N5SolucionContext()
        {
        }

        public N5SolucionContext(DbContextOptions<N5SolucionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Permissions> Permissions { get; set; }
        public virtual DbSet<PermissionsTypes> PermissionsTypes { get; set; }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permissions>(entity =>
            {
                entity.ToTable("Permissions", "n5Schema");

                entity.Property(e => e.EmployeeForename)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.EmployeeSurname)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.PermissionDate).HasColumnType("date");

                entity.HasOne(d => d.PermissionTypeNavigation)
                    .WithMany(p => p.Permissions)
                    .HasForeignKey(d => d.PermissionType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Permissions_PermissionsTypes");
            });

            modelBuilder.Entity<PermissionsTypes>(entity =>
            {
                entity.ToTable("PermissionsTypes", "n5Schema");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text");
            });

        }
    }
}
