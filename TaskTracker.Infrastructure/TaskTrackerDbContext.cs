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

                x.Property(a => a.Author)
                .IsRequired()
                .HasMaxLength(20);

                x.HasOne(t => t.MasterTask)
                .WithMany(t => t.SubTasks)
                .HasForeignKey(t => t.MasterTaskId)
                .IsRequired(false);


                x.ToTable("tasks");
            });

            builder.Entity<TaskRelationEntity>(x =>
            {
                x.HasKey(x => x.Id);

                x.ToTable("task_relations");
            });

            builder.Entity<TaskRelationship>(x =>
            {
                x.HasKey(x => new { x.RelationId, x.SourceTaskId, x.TargetTaskId });

                x.HasOne(rel => rel.Relation)
                .WithMany(trel => trel.Relationships)
                .HasForeignKey(rel => rel.RelationId);

                x.HasOne(rel => rel.SourceTask)
                .WithMany(t => t.OutRelations)
                .HasForeignKey(rel => rel.SourceTaskId)
                .OnDelete(DeleteBehavior.NoAction);

                x.HasOne(rel => rel.TargetTask)
                .WithMany(t => t.InRelations)
                .HasForeignKey(rel => rel.TargetTaskId)
                .OnDelete(DeleteBehavior.NoAction);

                x.ToTable("task_relationships");
            });
        }
    }
}
