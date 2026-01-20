using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BusinessWeb.Models.BusinessWebDB;

namespace BusinessWeb.Data
{
    public partial class BusinessWebDBContext : DbContext
    {
        public BusinessWebDBContext()
        {
        }

        public BusinessWebDBContext(DbContextOptions<BusinessWebDBContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BusinessWeb.Models.BusinessWebDB.AspNetUserLogin>().HasKey(table => new {
                table.LoginProvider, table.ProviderKey
            });

            builder.Entity<BusinessWeb.Models.BusinessWebDB.AspNetUserRole>().HasKey(table => new {
                table.UserId, table.RoleId
            });

            builder.Entity<BusinessWeb.Models.BusinessWebDB.AspNetUserToken>().HasKey(table => new {
                table.UserId, table.LoginProvider, table.Name
            });

            builder.Entity<BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim>()
              .HasOne(i => i.AspNetRole)
              .WithMany(i => i.AspNetRoleClaims)
              .HasForeignKey(i => i.RoleId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<BusinessWeb.Models.BusinessWebDB.AspNetUserClaim>()
              .HasOne(i => i.AspNetUser)
              .WithMany(i => i.AspNetUserClaims)
              .HasForeignKey(i => i.UserId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<BusinessWeb.Models.BusinessWebDB.AspNetUserLogin>()
              .HasOne(i => i.AspNetUser)
              .WithMany(i => i.AspNetUserLogins)
              .HasForeignKey(i => i.UserId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<BusinessWeb.Models.BusinessWebDB.AspNetUserRole>()
              .HasOne(i => i.AspNetRole)
              .WithMany(i => i.AspNetUserRoles)
              .HasForeignKey(i => i.RoleId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<BusinessWeb.Models.BusinessWebDB.AspNetUserRole>()
              .HasOne(i => i.AspNetUser)
              .WithMany(i => i.AspNetUserRoles)
              .HasForeignKey(i => i.UserId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<BusinessWeb.Models.BusinessWebDB.AspNetUserToken>()
              .HasOne(i => i.AspNetUser)
              .WithMany(i => i.AspNetUserTokens)
              .HasForeignKey(i => i.UserId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TAuthorize>()
              .Property(p => p.Societe)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TAuthorize>()
              .Property(p => p.SelectedAPP)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TAuthorize>()
              .Property(p => p.Visible)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TAuthorizeMobile>()
              .Property(p => p.Societe)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TAuthorizeMobile>()
              .Property(p => p.Visible)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TCollaborateur>()
              .Property(p => p.Societe)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TCollaborateur>()
              .Property(p => p.CO_No)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TDepot>()
              .Property(p => p.Societe)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TDepot>()
              .Property(p => p.DE_No)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TDepot>()
              .Property(p => p.Visible)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.Superficie)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.Capital)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.FormeJuridique)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.GestionCommercial)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.Rendement)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.RH)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.SousTraitance)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.Caisse)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.Banque)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.Recolte)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.RecolteSenegal)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.Region)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.Comptabilite)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.Tracabilite)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.HistoriqueConnexion)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.VersionSage)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSocieteUser>()
              .Property(p => p.Societe)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.AspNetUser>()
              .Property(p => p.LockoutEnd)
              .HasColumnType("datetimeoffset");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.DateDebut)
              .HasColumnType("smalldatetime");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.DateFin)
              .HasColumnType("smalldatetime");

            builder.Entity<BusinessWeb.Models.BusinessWebDB.TSociete>()
              .Property(p => p.DateCreation)
              .HasColumnType("smalldatetime");
            this.OnModelBuilding(builder);
        }

        public DbSet<BusinessWeb.Models.BusinessWebDB.AspNetRoleClaim> AspNetRoleClaims { get; set; }

        public DbSet<BusinessWeb.Models.BusinessWebDB.AspNetRole> AspNetRoles { get; set; }

        public DbSet<BusinessWeb.Models.BusinessWebDB.AspNetUserClaim> AspNetUserClaims { get; set; }

        public DbSet<BusinessWeb.Models.BusinessWebDB.AspNetUserLogin> AspNetUserLogins { get; set; }

        public DbSet<BusinessWeb.Models.BusinessWebDB.AspNetUserRole> AspNetUserRoles { get; set; }

        public DbSet<BusinessWeb.Models.BusinessWebDB.AspNetUser> AspNetUsers { get; set; }

        public DbSet<BusinessWeb.Models.BusinessWebDB.AspNetUserToken> AspNetUserTokens { get; set; }

        public DbSet<BusinessWeb.Models.BusinessWebDB.TAuthorize> TAuthorizes { get; set; }

        public DbSet<BusinessWeb.Models.BusinessWebDB.TAuthorizeMobile> TAuthorizeMobiles { get; set; }

        public DbSet<BusinessWeb.Models.BusinessWebDB.TCollaborateur> TCollaborateurs { get; set; }

        public DbSet<BusinessWeb.Models.BusinessWebDB.TDepot> TDepots { get; set; }

        public DbSet<BusinessWeb.Models.BusinessWebDB.TLicense> TLicenses { get; set; }

        public DbSet<BusinessWeb.Models.BusinessWebDB.TSociete> TSocietes { get; set; }

        public DbSet<BusinessWeb.Models.BusinessWebDB.TSocieteUser> TSocieteUsers { get; set; }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}