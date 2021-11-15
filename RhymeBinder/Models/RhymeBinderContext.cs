using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class RhymeBinderContext : DbContext
    {
        public RhymeBinderContext()
        {
        }

        public RhymeBinderContext(DbContextOptions<RhymeBinderContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<LnkTextSubmission> LnkTextSubmissions { get; set; }
        public virtual DbSet<Publication> Publications { get; set; }
        public virtual DbSet<PublicationRating> PublicationRatings { get; set; }
        public virtual DbSet<PublicationType> PublicationTypes { get; set; }
        public virtual DbSet<Submission> Submissions { get; set; }
        public virtual DbSet<SubmissionStatus> SubmissionStatuses { get; set; }
        public virtual DbSet<Text> Texts { get; set; }
        public virtual DbSet<TextGroup> TextGroups { get; set; }
        public virtual DbSet<TextHeader> TextHeaders { get; set; }
        public virtual DbSet<TextRecord> TextRecords { get; set; }
        public virtual DbSet<TextRevisionStatus> TextRevisionStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=RhymeBinder;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<LnkTextSubmission>(entity =>
            {
                entity.HasKey(e => e.LnkTextSumbissionId)
                    .HasName("PK__lnkTextS__077E7A660A0AF7AA");

                entity.ToTable("lnkTextSubmission");

                entity.Property(e => e.LnkTextSumbissionId).HasColumnName("lnkTextSumbissionID");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.SubmissionId).HasColumnName("SubmissionID");

                entity.Property(e => e.TextHeaderId).HasColumnName("TextHeaderID");

                entity.HasOne(d => d.Submission)
                    .WithMany(p => p.LnkTextSubmissions)
                    .HasForeignKey(d => d.SubmissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__lnkTextSu__Submi__4D2A7347");

                entity.HasOne(d => d.TextHeader)
                    .WithMany(p => p.LnkTextSubmissions)
                    .HasForeignKey(d => d.TextHeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__lnkTextSu__TextH__4C364F0E");
            });

            modelBuilder.Entity<Publication>(entity =>
            {
                entity.Property(e => e.PublicationId).HasColumnName("PublicationID");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PublicationRatingId).HasColumnName("PublicationRatingID");

                entity.Property(e => e.PublicationTypeId).HasColumnName("PublicationTypeID");

                entity.Property(e => e.Url)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("URL");

                entity.HasOne(d => d.PublicationRating)
                    .WithMany(p => p.Publications)
                    .HasForeignKey(d => d.PublicationRatingId)
                    .HasConstraintName("FK__Publicati__Publi__3552E9B6");

                entity.HasOne(d => d.PublicationType)
                    .WithMany(p => p.Publications)
                    .HasForeignKey(d => d.PublicationTypeId)
                    .HasConstraintName("FK__Publicati__Publi__345EC57D");
            });

            modelBuilder.Entity<PublicationRating>(entity =>
            {
                entity.Property(e => e.PublicationRatingId).HasColumnName("PublicationRatingID");

                entity.Property(e => e.PublicationRating1)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("PublicationRating");
            });

            modelBuilder.Entity<PublicationType>(entity =>
            {
                entity.Property(e => e.PublicationTypeId).HasColumnName("PublicationTypeID");

                entity.Property(e => e.PublicationType1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PublicationType");
            });

            modelBuilder.Entity<Submission>(entity =>
            {
                entity.Property(e => e.SubmissionId).HasColumnName("SubmissionID");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.PublicationId).HasColumnName("PublicationID");

                entity.Property(e => e.Reply).HasColumnType("datetime");

                entity.Property(e => e.SubmissionStatusId).HasColumnName("SubmissionStatusID");

                entity.Property(e => e.Submitted).HasColumnType("datetime");

                entity.Property(e => e.TextHeaderId).HasColumnName("TextHeaderID");

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.PublicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Submissio__Publi__477199F1");

                entity.HasOne(d => d.SubmissionStatus)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.SubmissionStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Submissio__Submi__4865BE2A");

                entity.HasOne(d => d.TextHeader)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.TextHeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Submissio__TextH__4959E263");
            });

            modelBuilder.Entity<SubmissionStatus>(entity =>
            {
                entity.Property(e => e.SubmissionStatusId).HasColumnName("SubmissionStatusID");

                entity.Property(e => e.SubmissionStatus1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SubmissionStatus");
            });

            modelBuilder.Entity<Text>(entity =>
            {
                entity.Property(e => e.TextId).HasColumnName("TextID");

                entity.Property(e => e.Created).HasColumnType("datetime");
            });

            modelBuilder.Entity<TextGroup>(entity =>
            {
                entity.Property(e => e.TextGroupId).HasColumnName("TextGroupID");

                entity.Property(e => e.GroupTitle)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TextHeader>(entity =>
            {
                entity.Property(e => e.TextHeaderId).HasColumnName("TextHeaderID");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.LastRead).HasColumnType("datetime");

                entity.Property(e => e.TextGroupId).HasColumnName("TextGroupID");

                entity.Property(e => e.TextId).HasColumnName("TextID");

                entity.Property(e => e.TextRevisionStatusId).HasColumnName("TextRevisionStatusID");

                entity.Property(e => e.Title)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.TextGroup)
                    .WithMany(p => p.TextHeaders)
                    .HasForeignKey(d => d.TextGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TextHeade__TextG__3DE82FB7");

                entity.HasOne(d => d.Text)
                    .WithMany(p => p.TextHeaders)
                    .HasForeignKey(d => d.TextId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TextHeade__TextI__3EDC53F0");

                entity.HasOne(d => d.TextRevisionStatus)
                    .WithMany(p => p.TextHeaders)
                    .HasForeignKey(d => d.TextRevisionStatusId)
                    .HasConstraintName("FK__TextHeade__TextR__3FD07829");

                entity.HasOne(d => d.VersionOfNavigation)
                    .WithMany(p => p.InverseVersionOfNavigation)
                    .HasForeignKey(d => d.VersionOf)
                    .HasConstraintName("FK__TextHeade__Versi__40C49C62");
            });

            modelBuilder.Entity<TextRecord>(entity =>
            {
                entity.ToTable("TextRecord");

                entity.Property(e => e.TextRecordId).HasColumnName("TextRecordID");

                entity.Property(e => e.Recorded).HasColumnType("datetime");

                entity.Property(e => e.TextHeaderId).HasColumnName("TextHeaderID");

                entity.Property(e => e.TextId).HasColumnName("TextID");

                entity.HasOne(d => d.TextHeader)
                    .WithMany(p => p.TextRecords)
                    .HasForeignKey(d => d.TextHeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TextRecor__TextH__43A1090D");

                entity.HasOne(d => d.Text)
                    .WithMany(p => p.TextRecords)
                    .HasForeignKey(d => d.TextId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TextRecor__TextI__44952D46");
            });

            modelBuilder.Entity<TextRevisionStatus>(entity =>
            {
                entity.Property(e => e.TextRevisionStatusId).HasColumnName("TextRevisionStatusID");

                entity.Property(e => e.TextRevisionStatus1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TextRevisionStatus");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
