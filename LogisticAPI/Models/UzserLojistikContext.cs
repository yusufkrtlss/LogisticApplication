using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LogisticApi.Models;

public partial class UzserLojistikContext : DbContext
{
    public UzserLojistikContext()
    {
    }

    public UzserLojistikContext(DbContextOptions<UzserLojistikContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }


    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<ProgramParameter> ProgramParameters { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleModule> RoleModules { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCompany> UserCompanies { get; set; }

    public virtual DbSet<UserModule> UserModules { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=192.168.1.232,1433; Database=UzserLojistik;User Id=sa;Password=sapass; Trusted_Connection=false;TrustServerCertificate=true;MultipleActiveResultSets=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(512);
            entity.Property(e => e.CompanyName).HasMaxLength(512);
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            entity.Property(e => e.TaxNumber)
                .HasMaxLength(11)
                .IsUnicode(false);
        });


        modelBuilder.Entity<Module>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ModuleName).HasMaxLength(255);
            entity.Property(e => e.ModuleNameEn)
                .HasMaxLength(255)
                .HasColumnName("ModuleName_EN");
        });

        modelBuilder.Entity<ProgramParameter>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ParameterName).HasMaxLength(100);
            entity.Property(e => e.ParameterValue).HasMaxLength(512);

          
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            entity.Property(e => e.RoleName).HasMaxLength(255);
            entity.Property(e => e.RoleNameEn)
                .HasMaxLength(255)
                .HasColumnName("RoleName_EN");
        });

        modelBuilder.Entity<RoleModule>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");

           

            entity.HasOne(d => d.Module).WithMany(p => p.RoleModules)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleModules_Modules");

            entity.HasOne(d => d.Role).WithMany(p => p.RoleModules)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleModules_Roles");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email)
                .HasMaxLength(512)
                .HasColumnName("EMail");
            entity.Property(e => e.FullName).HasMaxLength(512);
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.RefreshTokenEndDate).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(512)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserCompany>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");

          

            entity.HasOne(d => d.User).WithMany(p => p.UserCompanies)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCompanies_Users");
        });

        modelBuilder.Entity<UserModule>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

          
            entity.HasOne(d => d.Module).WithMany(p => p.UserModules)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserModules_Modules");

            entity.HasOne(d => d.User).WithMany(p => p.UserModules)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserModules_Users");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");

        

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Roles");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Users");
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ClientName).HasMaxLength(512);
            entity.Property(e => e.LastAccessDate).HasColumnType("datetime");
            entity.Property(e => e.LoginDate).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(512);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
