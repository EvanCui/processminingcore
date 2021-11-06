using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Encoo.ProcessMining.DataContext.Model;

#nullable disable

namespace Encoo.ProcessMining.DataContext.DatabaseContext
{
    public partial class ProcessMiningDatabaseContext : DbContext
    {
        public ProcessMiningDatabaseContext()
        {
        }

        public ProcessMiningDatabaseContext(DbContextOptions<ProcessMiningDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActivityDefinition> ActivityDefinitions { get; set; }
        public virtual DbSet<ActivityDetectionRule> ActivityDetectionRules { get; set; }
        public virtual DbSet<ActivityInstance> ActivityInstances { get; set; }
        public virtual DbSet<DataRecord> DataRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ActivityDefinition>(entity =>
            {
                entity.ToTable("ActivityDefinition");

                entity.Property(e => e.Details).HasMaxLength(1000);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ActivityDetectionRule>(entity =>
            {
                entity.ToTable("ActivityDetectionRule");

                entity.HasIndex(e => e.ActivityDefinitionId, "IX_ActivityDetectionRule_ActivityDefinitionId")
                    .IsUnique();

                entity.HasIndex(e => new { e.Priority, e.Id }, "IX_ActivityDetectionRule_Priority_Id");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.RuleData).HasColumnType("text");

                entity.HasOne(d => d.ActivityDefinition)
                    .WithOne(p => p.ActivityDetectionRule)
                    .HasForeignKey<ActivityDetectionRule>(d => d.ActivityDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActivityDetectionRule_ActivityDefinition");
            });

            modelBuilder.Entity<ActivityInstance>(entity =>
            {
                entity.ToTable("ActivityInstance");

                entity.HasIndex(e => new { e.ProcessSubject, e.Time, e.Id }, "IX_ActivityInstance_ProcessSubject_Time_Id");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Actor).HasMaxLength(50);

                entity.Property(e => e.ProcessSubject)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.ActivityDefinition)
                    .WithMany(p => p.ActivityInstances)
                    .HasForeignKey(d => d.ActivityDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActivityInstance_ActivityDefinition");

                entity.HasOne(d => d.DataRecord)
                    .WithMany(p => p.ActivityInstances)
                    .HasForeignKey(d => d.DataRecordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActivityInstance_DataRecord");

                entity.HasOne(d => d.DetectionRule)
                    .WithMany(p => p.ActivityInstances)
                    .HasForeignKey(d => d.DetectionRuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActivityInstance_ActivityDetectionRule");
            });

            modelBuilder.Entity<DataRecord>(entity =>
            {
                entity.ToTable("DataRecord");

                entity.HasIndex(e => new { e.IsDeleted, e.IsTemplateDetected, e.IsActivityDetected, e.KnowledgeWatermark }, "IX_DataRecord_IsDeleted_IsTemplateDetected_IsActivityDetected_KnowledgeWatermark");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.Parameters).HasMaxLength(1000);

                entity.Property(e => e.Template).HasMaxLength(1000);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
