using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Domain.Abstractions;
using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.Infrastructure
{
    public sealed class TaskTrackerDbContext : DbContext, IUnitOfWork
    {
        public TaskTrackerDbContext(DbContextOptions<TaskTrackerDbContext> options) : base(options)
        {
        }

        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<TaskRelationEntity> TaskRelations { get; set; }
        public DbSet<TaskRelationship> TaskRelationships { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TaskEntity>(x =>
            {
                x.HasKey(x => x.Id);

                x.Property(x => x.Author).IsRequired();

                x.HasOne(x => x.MasterTask)
                .WithMany(x => x.SubTasks)
                .HasForeignKey(x => x.MasterTaskId)
                .IsRequired(false);

                x.ToTable("tasks");
            });

            builder.Entity<TaskRelationEntity>(x =>
            {
                x.HasKey(x => x.Id);

                x.HasMany(x => x.Relationships)
                .WithOne(x => x.Relation)
                .HasForeignKey(x => x.RelationId);

                x.ToTable("task_relations");
            });

            builder.Entity<TaskRelationship>(x =>
            {
                x.HasKey(x => new { x.RelationId, x.LeftTaskId, x.RightTaskId });

                x.HasOne(x => x.LeftTask)
                .WithMany()
                .HasForeignKey(x => x.LeftTaskId)
                .OnDelete(DeleteBehavior.NoAction);

                x.HasOne(x => x.RightTask)
                .WithMany()
                .HasForeignKey(x => x.RightTaskId)
                .OnDelete(DeleteBehavior.NoAction);

                x.ToTable("task_relationships");
            });
        }
    }
}
