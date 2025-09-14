using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Domain.Abstractions;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.Infrastructure
{
    public sealed class TaskTrackerDbContext : DbContext, IUnitOfWork
    {
        public const string WorkAssignmentsTableName = "work_assignments";
        public const string WorkAssignmentRelationshipsTableName = "work_assignments_relationships";
        public const int MaxAuthorLength = 100;

        public TaskTrackerDbContext(DbContextOptions<TaskTrackerDbContext> options) : base(options)
        {
        }

        public DbSet<WorkAssignment> WorkAssignments { get; set; }
        public DbSet<WorkAssignmentRelationship> WorkAssignmentRelationships { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<WorkAssignment>(x =>
            {
                x.HasKey(x => x.Id);

                x.Property(a => a.Author)
                .IsRequired()
                .HasMaxLength(MaxAuthorLength);

                x.HasOne(t => t.HeadAssignment)
                .WithMany(t => t.SubAssignment)
                .HasForeignKey(t => t.HeadAssignmentId)
                .IsRequired(false);

                x.ToTable(WorkAssignmentsTableName);
            });

            builder.Entity<WorkAssignmentRelationship>(x =>
            {
                x.HasKey(x => new { x.Relation, x.SourceWorkAssignmentId, x.TargetWorkAssignmentId });

                x.HasIndex(x => new { x.Relation, x.SourceWorkAssignmentId, x.TargetWorkAssignmentId })
                .IsUnique();

                x.HasOne(rel => rel.SourceWorkAssignment)
                .WithMany(t => t.OutRelations)
                .HasForeignKey(rel => rel.SourceWorkAssignmentId)
                .OnDelete(DeleteBehavior.NoAction);

                x.HasOne(rel => rel.TargetWorkAssignment)
                .WithMany(t => t.InRelations)
                .HasForeignKey(rel => rel.TargetWorkAssignmentId)
                .OnDelete(DeleteBehavior.NoAction);

                x.ToTable(WorkAssignmentRelationshipsTableName);
            });
        }
    }
}
