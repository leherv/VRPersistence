﻿using Microsoft.EntityFrameworkCore;
using VRPersistence.DAO;

namespace VRPersistence
{
    public class VRPersistenceDbContext : DbContext
    {
        public DbSet<Release> Releases { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<NotificationEndpoint> NotificationEndpoints { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public VRPersistenceDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Media>(e => e
                .HasIndex(m => m.MediaName)
                .IsUnique()
            );
            modelBuilder.Entity<NotificationEndpoint>(e => e
                .HasIndex(n => n.Identifier)
                .IsUnique()
            );
            modelBuilder.Entity<Subscription>(e => e
                .HasIndex(s => new {s.MediaId, s.NotificationEndpointId})
                .IsUnique()
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}