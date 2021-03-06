using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Encoo.ProcessMining.DataContext.Model;

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
        public virtual DbSet<ProcessCluster> ProcessClusters { get; set; }
        public virtual DbSet<ProcessClusterActivityDefinitionRelationship> ProcessClusterActivityDefinitionRelationships { get; set; }
        public virtual DbSet<ProcessDefinition> ProcessDefinitions { get; set; }
        public virtual DbSet<ProcessGroup> ProcessGroups { get; set; }
        public virtual DbSet<ProcessInstance> ProcessInstances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityDefinition>(entity =>
            {
                entity.ToTable("ActivityDefinition");

                entity.HasIndex(e => e.ProcessDefinitionId, "IX_ActivityDefinition_ProcessDefinitionId");

                entity.Property(e => e.Details).HasMaxLength(1000);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.ProcessDefinition)
                    .WithMany(p => p.ActivityDefinitions)
                    .HasForeignKey(d => d.ProcessDefinitionId)
                    .HasConstraintName("FK_ActivityDefinition_ProcessDefinition");
            });

            modelBuilder.Entity<ActivityDetectionRule>(entity =>
            {
                entity.ToTable("ActivityDetectionRule");

                entity.HasIndex(e => e.ActivityDefinitionId, "IX_ActivityDetectionRule_ActivityDefinitionId")
                    .IsUnique();

                entity.HasIndex(e => new { e.Priority, e.Id }, "IX_ActivityDetectionRule_Priority_Id");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.RuleData).HasColumnType("text");

                entity.HasOne(d => d.ActivityDefinition)
                    .WithOne(p => p.ActivityDetectionRule)
                    .HasForeignKey<ActivityDetectionRule>(d => d.ActivityDefinitionId)
                    .HasConstraintName("FK_ActivityDetectionRule_ActivityDefinition");
            });

            modelBuilder.Entity<ActivityInstance>(entity =>
            {
                entity.ToTable("ActivityInstance");

                entity.HasIndex(e => new { e.ProcessInstanceId, e.ProcessSubject }, "IX_ActivityInstance_ProcessInstanceId_ProcessSubject");

                entity.Property(e => e.Actor).HasMaxLength(50);

                entity.Property(e => e.ProcessSubject)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.ActivityDefinition)
                    .WithMany(p => p.ActivityInstances)
                    .HasForeignKey(d => d.ActivityDefinitionId)
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

                entity.HasOne(d => d.ProcessInstance)
                    .WithMany(p => p.ActivityInstances)
                    .HasForeignKey(d => d.ProcessInstanceId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_ActivityInstance_ProcessInstance");
            });

            modelBuilder.Entity<DataRecord>(entity =>
            {
                entity.ToTable("DataRecord");

                entity.HasIndex(e => new { e.IsDeleted, e.IsTemplateDetected, e.IsActivityDetected, e.KnowledgeWatermark }, "IX_DataRecord_IsDeleted_IsTemplateDetected_IsActivityDetected_KnowledgeWatermark");

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.Parameters).HasColumnType("text");

                entity.Property(e => e.Template).HasColumnType("text");
            });

            modelBuilder.Entity<ProcessCluster>(entity =>
            {
                entity.ToTable("ProcessCluster");

                entity.HasIndex(e => e.Thumbprint, "IX_ProcessCluster_Thumbprint")
                    .IsUnique();

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Thumbprint)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ProcessClusterActivityDefinitionRelationship>(entity =>
            {
                entity.HasKey(e => e.ProcessClusterId)
                    .HasName("PK__ProcessC__76FFA923509B8EDF");

                entity.ToTable("ProcessClusterActivityDefinitionRelationship");

                entity.HasIndex(e => e.ActivityDefinitionId, "IX_ProcessClusterActivityDefinitionRelationship_ActivityDefinitionId");

                entity.Property(e => e.ProcessClusterId).ValueGeneratedNever();

                entity.HasOne(d => d.ActivityDefinition)
                    .WithMany(p => p.ProcessClusterActivityDefinitionRelationships)
                    .HasForeignKey(d => d.ActivityDefinitionId)
                    .HasConstraintName("FK_ProcessClusterActivityDefinitionRelationship_ActivityDefinition");

                entity.HasOne(d => d.ProcessCluster)
                    .WithOne(p => p.ProcessClusterActivityDefinitionRelationship)
                    .HasForeignKey<ProcessClusterActivityDefinitionRelationship>(d => d.ProcessClusterId)
                    .HasConstraintName("FK_ProcessClusterActivityDefinitionRelationship_ProcessCluster");
            });

            modelBuilder.Entity<ProcessDefinition>(entity =>
            {
                entity.ToTable("ProcessDefinition");

                entity.Property(e => e.Details).HasMaxLength(1000);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ProcessGroup>(entity =>
            {
                entity.ToTable("ProcessGroup");

                entity.HasIndex(e => e.Thumbprint, "IX_ProcessGroup_Thumbprint")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Thumbprint)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ProcessInstance>(entity =>
            {
                entity.ToTable("ProcessInstance");

                entity.HasIndex(e => new { e.IsClustered, e.Thumbprint }, "IX_ProcessInstance_IsGrouped_Thumbprint");

                entity.HasIndex(e => e.Subject, "IX_ProcessInstance_Subject")
                    .IsUnique();

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Thumbprint)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.ProcessCluster)
                    .WithMany(p => p.ProcessInstances)
                    .HasForeignKey(d => d.ProcessClusterId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_ProcessInstance_ProcessCluster");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
