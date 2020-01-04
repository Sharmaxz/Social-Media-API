using System;
using System.Data.Entity;
using SocialMediaApi.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SocialMediaApi.DAL
{
    public class SocialMediaContext : DbContext
    {
        public SocialMediaContext() : base ("Social-Media_db")
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<SocialMediaContext, Migrations.Configuration>());
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Commentaries { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}