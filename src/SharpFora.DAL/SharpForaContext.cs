using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.Configuration;
using SharpFora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpFora.DAL
{
    public class SharpForaContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        private IConfigurationRoot Config { get; } =
            new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        protected override void OnConfiguring(DbContextOptionsBuilder builder) =>
            builder.UseSqlServer(Config["EntityFramework:SharpForaContext:ConnectionStringKey"]);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Add Index to User.Name
            builder.Entity<User>()
                .HasIndex(x => x.Name)
                .IsUnique(true);
            // Assign composite primary key
            builder.Entity<UserRole>()
                .HasKey(x => new { x.RoleId, x.UserId });
            // Create User to Role binding
            //builder.Entity<UserRole>()
            //    .HasOne(x => x.User)
            //    .WithMany(x => x.UserRoles)
            //    .HasForeignKey(x => x.UserId);
            //builder.Entity<UserRole>()
            //    .HasOne(x => x.Role)
            //    .WithMany(x => x.UserRoles)
            //    .HasForeignKey(x => x.RoleId);
        }
    }
}
