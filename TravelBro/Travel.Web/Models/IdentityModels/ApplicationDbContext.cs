﻿using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.Migrations;
using WebApplication1.Models.EntityModels;

namespace WebApplication1.Models.IdentityModels
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private static bool _dbInitialized = false;

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Initialize();
        }

        private static void Initialize()
        {
            if (!_dbInitialized)
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
                _dbInitialized = true;
            }
        }
        
        public static ApplicationDbContext CreateInstance()
        {
            Initialize();
            return new ApplicationDbContext();
        }
        
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Trip>()
                .HasRequired(t => t.Author)
                .WithMany(a => a.Trips)
                .Map(m => m.MapKey("AuthorId"))
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ApplicationUser>()
                .Ignore(p => p.Trips)
                .Ignore(p => p.Photos)
                .Ignore(p => p.Comments);

            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Comments)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("CommentsToTrips");
                    m.MapLeftKey("TripId");
                    m.MapRightKey("CommentId");
                });

            #region Visit

            modelBuilder.Entity<Visit>()
                .HasRequired(v => v.Trip)
                .WithMany(t => t.Visits)
                .Map(m => m.MapKey("TripId"));

            modelBuilder.Entity<Visit>()
                .HasMany(t => t.Comments)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("CommentsToVisits");
                    m.MapLeftKey("VisitId");
                    m.MapRightKey("CommentId");
                });
            #endregion

            #region Route

            modelBuilder.Entity<Route>()
                .HasRequired(v => v.Trip)
                .WithMany(t => t.Routes)
                .Map(m => m.MapKey("TripId"));

            modelBuilder.Entity<Route>()
                .HasMany(t => t.Comments)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("CommentsToRoutes");
                    m.MapLeftKey("RouteId");
                    m.MapRightKey("CommentId");
                });
            #endregion

            #region Comments & Photos
            modelBuilder.Entity<Comment>()
                    .HasRequired(c => c.Author)
                    .WithMany(a => a.Comments)
                    .Map(m => m.MapKey("AuthorId"))
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comment>()
                .HasOptional(c => c.ReplyTo)
                .WithOptionalDependent()
                .Map(m => m.MapKey("ReplyToCommentId"))
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Photo>()
                .HasRequired(p => p.Author)
                .WithMany(a => a.Photos)
                .Map(m => m.MapKey("AuthorId"))
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Photo>()
                .HasMany(t => t.Comments)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("CommentsToPhotos");
                    m.MapLeftKey("PhotoId");
                    m.MapRightKey("CommentId");
                });
            #endregion

            #region PhotosToX

            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Photos)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("PhotosToTrips");
                    m.MapLeftKey("TripId");
                    m.MapRightKey("PhotoId");
                });

            modelBuilder.Entity<Visit>()
                .HasMany(t => t.Photos)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("PhotosToVisits");
                    m.MapLeftKey("VisitId");
                    m.MapRightKey("PhotoId");
                });

            modelBuilder.Entity<Route>()
                .HasMany(t => t.Photos)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("PhotosToRoutes");
                    m.MapLeftKey("RouteId");
                    m.MapRightKey("PhotoId");
                });

            #endregion

        }
    }
}