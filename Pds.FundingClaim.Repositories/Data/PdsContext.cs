using Microsoft.EntityFrameworkCore;
using Pds.FundingClaim.Repositories.DataModels;

namespace Pds.FundingClaim.Repositories.Data
{
    /// <summary>
    /// The database context.
    /// </summary>
    public partial class PdsContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PdsContext"/> class.
        /// The parameterised constructor.
        /// </summary>
        /// <param name="options">Context options to set up sql server connection.</param>
        public PdsContext(DbContextOptions<PdsContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets FundingClaimData database entity.
        /// </summary>
        public virtual DbSet<FundingClaimData> FundingClaimDatas { get; set; }

        /// <summary>
        /// Gets or sets FundingClaimWindows database entity.
        /// </summary>
        public virtual DbSet<FundingClaimWindow> FundingClaimWindows { get; set; }

        /// <summary>
        /// Gets or sets FundingClaim database entity.
        /// </summary>
        public virtual DbSet<DataModels.FundingClaim> FundingClaims { get; set; }

        /// <summary>
        /// Gets or sets Settings database entity.
        /// </summary>
        public virtual DbSet<Setting> Settings { get; set; }

        public virtual DbSet<ReconciliationData> ReconciliationData { get; set; }

        public virtual DbSet<Reconciliations> Reconciliations { get; set; }

        public virtual DbSet<ReconciliationAllocationGroups> ReconciliationAllocationGroups { get; set; }

        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FundingClaimData>(entity =>
            {
                entity.ToTable("FundingClaimDatas", "Contracts");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("IX_Id");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.FundingClaim)
                    .WithOne(p => p.FundingClaimData)
                    .HasForeignKey<FundingClaimData>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contracts.FundingClaimDatas_Contracts.FundingClaims_Id");
            });

            modelBuilder.Entity<FundingClaimWindow>(entity =>
            {
                entity.ToTable("FundingClaimWindows", "Contracts");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DataCollectionKey).IsRequired();

                entity.Property(e => e.LastUpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.SignatureCloseDate).HasColumnType("datetime");

                entity.Property(e => e.SubmissionCloseDate).HasColumnType("datetime");

                entity.Property(e => e.SubmissionOpenDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<DataModels.FundingClaim>(entity =>
            {
                entity.ToTable("FundingClaims", "Contracts");

                entity.HasIndex(e => e.FundingClaimWindowId)
                    .HasDatabaseName("IX_FundingClaimWindow_Id");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DateSubmitted).HasColumnType("datetime");

                entity.Property(e => e.FundingClaimUniqueId)
                    .IsRequired()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FundingClaimWindowId)
                    .HasColumnName("FundingClaimWindow_Id")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastUpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.Period).IsRequired();

                entity.Property(e => e.SignedOn).HasColumnType("datetime");

                entity.Property(e => e.Title).IsRequired();

                entity.HasOne(d => d.FundingClaimWindow)
                    .WithMany(p => p.FundingClaims)
                    .HasForeignKey(d => d.FundingClaimWindowId)
                    .HasConstraintName("FK_Contracts.FundingClaims_Contracts.FundingClaimWindows_FundingClaimWindow_Id");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("Settings", "Contracts");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.Value).IsRequired();
            });

            modelBuilder.Entity<ReconciliationData>(entity =>
            {
                entity.ToTable("ReconciliationData", "Contracts");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("IX_Id");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.ReconciliationData)
                    .HasForeignKey<ReconciliationData>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contracts.ReconciliationData_Contracts.Reconciliations_Id");
            });

            modelBuilder.Entity<Reconciliations>(entity =>
            {
                entity.ToTable("Reconciliations", "Contracts");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.IsValid)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastUpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.Period).IsRequired();

                entity.Property(e => e.Title).IsRequired();
            });

            modelBuilder.Entity<ReconciliationAllocationGroups>(entity =>
            {
                entity.ToTable("ReconciliationAllocationGroups", "Contracts");

                entity.Property(e => e.Code).IsRequired();

                entity.Property(e => e.Description).IsRequired();
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}