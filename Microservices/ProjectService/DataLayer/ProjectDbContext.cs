﻿using Microsoft.EntityFrameworkCore;
using SharedLibrary.Entities;
using SharedLibrary.Entities.ProjectService;
using System.Collections.Generic;

namespace ProjectService.DataLayer
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options) { }

        public DbSet<AttachmentEntity> Attachments => Set<AttachmentEntity>();
        public DbSet<BoardEntity> Boards => Set<BoardEntity>();
        public DbSet<CommentEntity> Comments => Set<CommentEntity>();

        public DbSet<ItemEntity> Items => Set<ItemEntity>();
        public DbSet<ItemBoardEntity> ItemsBoards => Set<ItemBoardEntity>();
        public DbSet<ItemTypeEntity> ItemTypes => Set<ItemTypeEntity>();
        public DbSet<SprintEntity> Sprints => Set<SprintEntity>();
        public DbSet<StatusEntity> Statuses => Set<StatusEntity>();
        public DbSet<RoleEntity> Roles => Set<RoleEntity>();
        public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
        public DbSet<UserItemEntity> UserItems => Set<UserItemEntity>();
        public DbSet<UserProjectEntity> UserProjects => Set<UserProjectEntity>();
        public DbSet<DocumentEntity> Documents => Set<DocumentEntity>();
        public DbSet<ProjectLinkEntity> VisibilityLinks => Set<ProjectLinkEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProjectEntity>()
                .HasKey(up => new { up.Id});

            // Устанавливаем первичный ключ для UserProjectEntity
            modelBuilder.Entity<UserProjectEntity>()
                .HasKey(up => new { up.Id });
            
            modelBuilder.Entity<UserProjectEntity>()
                .HasOne(up => up.Project)
                .WithMany(p => p.UserProjects)
                .HasForeignKey(up => up.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserProjectEntity>()
                .HasOne(up => up.Role)
                .WithMany(r => r.UserProjects)
                .HasForeignKey(up => up.RoleId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UserItemEntity>()
                .HasOne(up => up.Item)
                .WithMany(i => i.UserItems)
                .HasForeignKey(up => up.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemBoardEntity>()
                .HasKey(ib => new { ib.ItemId, ib.BoardId });

            modelBuilder.Entity<ItemBoardEntity>()
                .HasOne(ib => ib.Item)
                .WithMany(i => i.ItemsBoards)
                .HasForeignKey(ib => ib.ItemId);

            modelBuilder.Entity<ItemBoardEntity>()
                .HasOne(ib => ib.Board)
                .WithMany(b => b.ItemsBoards)
                .HasForeignKey(ib => ib.BoardId);

            modelBuilder.Entity<ItemBoardEntity>()
                .HasOne(ib => ib.Status)
                .WithMany(b => b.ItemsBoards)
                .HasForeignKey(ib => ib.StatusId);

            modelBuilder.Entity<ProjectLinkEntity>()
                .HasOne(v => v.Project)
                .WithMany(p => p.VisibilityLinks)
                .HasForeignKey(v => v.ProjectId);

            // projects -> boards (project_id)
            modelBuilder.Entity<BoardEntity>()
                .HasOne(b=>b.Project)
                .WithMany(p => p.Boards)
                .HasForeignKey(b => b.ProjectId);

            // status -> boards (status_id)
            modelBuilder.Entity<BoardEntity>()
                .HasMany(b => b.Statuses)
                .WithOne(s => s.Board)
                .HasForeignKey(b => b.BoardId);

            // items -> items (parent_id)
            modelBuilder.Entity<ItemEntity>()
                .HasOne(i => i.Parent)
                .WithMany(i => i.Children)
                .HasForeignKey(i => i.ParentId);

            // items -> projects (project_id)
            modelBuilder.Entity<ItemEntity>()
                .HasOne(i => i.Project)
                .WithMany(p => p.Items)
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // items -> item_type (item_type_id)
            modelBuilder.Entity<ItemEntity>()
                .HasOne(i => i.ItemType)
                .WithMany(it => it.Items)
                .HasForeignKey(i => i.ItemTypeId);

            // items -> status (status_id)
            modelBuilder.Entity<ItemEntity>()
                .HasOne(i => i.Status)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.StatusId);

            // comments -> items (item_id)
            modelBuilder.Entity<CommentEntity>()
                .HasOne(c => c.Item)
                .WithMany(i => i.Comments)
                .HasForeignKey(c => c.ItemId);

            // attachments -> comments (comment_id)
            modelBuilder.Entity<AttachmentEntity>()
                .HasOne(a => a.Comment)
                .WithMany(c => c.Attachments)
                .HasForeignKey(a => a.CommentId);

            // documents -> projects (project_id)
            modelBuilder.Entity<DocumentEntity>()
                .HasOne(d => d.Project)
                .WithMany(p => p.Documents)
                .HasForeignKey(d => d.ProjectId);

            // sprints -> boards (board_id)
            modelBuilder.Entity<SprintEntity>()
                .HasOne(s => s.Board)
                .WithMany(b => b.Sprints)
                .HasForeignKey(s => s.BoardId);

            modelBuilder.Entity<ItemEntity>()
                        .HasMany(i => i.Sprints)
                        .WithMany(s => s.Items)
                        .UsingEntity<Dictionary<string, object>>(
                            "items_sprints",
                            j => j
                                .HasOne<SprintEntity>()
                                .WithMany()
                                .HasForeignKey("sprint_id"),
                            j => j
                                .HasOne<ItemEntity>()
                                .WithMany()
                                .HasForeignKey("item_id"));
        }

    }
}
