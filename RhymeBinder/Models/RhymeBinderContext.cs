using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Binder> Binders { get; set; }
        public virtual DbSet<EditWindowProperty> EditWindowProperties { get; set; }
        public virtual DbSet<GroupAction> GroupActions { get; set; }
        public virtual DbSet<GroupHistory> GroupHistories { get; set; }
        public virtual DbSet<LnkTextHeadersTextGroup> LnkTextHeadersTextGroups { get; set; }
        public virtual DbSet<LnkTextSubmission> LnkTextSubmissions { get; set; }
        public virtual DbSet<Publication> Publications { get; set; }
        public virtual DbSet<PublicationRating> PublicationRatings { get; set; }
        public virtual DbSet<PublicationType> PublicationTypes { get; set; }
        public virtual DbSet<SavedView> SavedViews { get; set; }
        public virtual DbSet<SimpleUser> SimpleUsers { get; set; }
        public virtual DbSet<Submission> Submissions { get; set; }
        public virtual DbSet<SubmissionStatus> SubmissionStatuses { get; set; }
        public virtual DbSet<Text> Texts { get; set; }
        public virtual DbSet<TextGroup> TextGroups { get; set; }
        public virtual DbSet<TextHeader> TextHeaders { get; set; }
        public virtual DbSet<TextHeaderTitleDefaultType> TextHeaderTitleDefaultTypes { get; set; }
        public virtual DbSet<TextRecord> TextRecords { get; set; }
        public virtual DbSet<TextRevisionStatus> TextRevisionStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=RhymeBinder;Trusted_Connection=True;Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
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

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Binder>(entity =>
            {
                entity.Property(e => e.BinderId).HasColumnName("BinderID");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.LastAccessed).HasColumnType("datetime");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.NewTextDefaultShowLineCount)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.NewTextDefaultShowParagraphCount)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TextHeaderTitleDefaultType).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.BinderCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Binders__Created__664B26CC");

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.BinderLastModifiedByNavigations)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .HasConstraintName("FK__Binders__LastMod__673F4B05");

                entity.HasOne(d => d.TextHeaderTitleDefaultTypeNavigation)
                    .WithMany(p => p.Binders)
                    .HasForeignKey(d => d.TextHeaderTitleDefaultType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Binders__TextHea__5AA469F6");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BinderUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Binders__UserID__65570293");
            });

            modelBuilder.Entity<EditWindowProperty>(entity =>
            {
                entity.Property(e => e.EditWindowPropertyId).HasColumnName("EditWindowPropertyID");

                entity.Property(e => e.ActiveElement)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TextHeaderId).HasColumnName("TextHeaderID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.TextHeader)
                    .WithMany(p => p.EditWindowProperties)
                    .HasForeignKey(d => d.TextHeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EditWindo__TextH__19FFD4FC");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EditWindowProperties)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__EditWindo__UserI__190BB0C3");
            });

            modelBuilder.Entity<GroupAction>(entity =>
            {
                entity.Property(e => e.GroupActionId).HasColumnName("GroupActionID");

                entity.Property(e => e.GroupAction1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("GroupAction");
            });

            modelBuilder.Entity<GroupHistory>(entity =>
            {
                entity.HasKey(e => e.GroupHistoryLogId)
                    .HasName("PK__GroupHis__85BD91A4142A3085");

                entity.ToTable("GroupHistory");

                entity.Property(e => e.GroupHistoryLogId).HasColumnName("GroupHistoryLogID");

                entity.Property(e => e.DateLogged).HasColumnType("datetime");

                entity.Property(e => e.GroupActionId).HasColumnName("GroupActionID");

                entity.Property(e => e.TextGroupId).HasColumnName("TextGroupID");

                entity.Property(e => e.TextHeaderId).HasColumnName("TextHeaderID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.GroupAction)
                    .WithMany(p => p.GroupHistories)
                    .HasForeignKey(d => d.GroupActionId)
                    .HasConstraintName("FK__GroupHist__Group__627A95E8");

                entity.HasOne(d => d.TextGroup)
                    .WithMany(p => p.GroupHistories)
                    .HasForeignKey(d => d.TextGroupId)
                    .HasConstraintName("FK__GroupHist__TextG__618671AF");

                entity.HasOne(d => d.TextHeader)
                    .WithMany(p => p.GroupHistories)
                    .HasForeignKey(d => d.TextHeaderId)
                    .HasConstraintName("FK__GroupHist__TextH__60924D76");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupHistories)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__GroupHist__UserI__5F9E293D");
            });

            modelBuilder.Entity<LnkTextHeadersTextGroup>(entity =>
            {
                entity.HasKey(e => e.LnkHeaderGroupId)
                    .HasName("PK__lnkTextH__F316D0747BD4CEF5");

                entity.ToTable("lnkTextHeadersTextGroups");

                entity.Property(e => e.LnkHeaderGroupId).HasColumnName("lnkHeaderGroupID");

                entity.Property(e => e.TextGroupId).HasColumnName("TextGroupID");

                entity.Property(e => e.TextHeaderId).HasColumnName("TextHeaderID");

                entity.HasOne(d => d.TextGroup)
                    .WithMany(p => p.LnkTextHeadersTextGroups)
                    .HasForeignKey(d => d.TextGroupId)
                    .HasConstraintName("FK__lnkTextHe__TextG__43F60EC8");

                entity.HasOne(d => d.TextHeader)
                    .WithMany(p => p.LnkTextHeadersTextGroups)
                    .HasForeignKey(d => d.TextHeaderId)
                    .HasConstraintName("FK__lnkTextHe__TextH__4301EA8F");
            });

            modelBuilder.Entity<LnkTextSubmission>(entity =>
            {
                entity.HasKey(e => e.LnkTextSumbissionId)
                    .HasName("PK__lnkTextS__077E7A668E16338E");

                entity.ToTable("lnkTextSubmission");

                entity.Property(e => e.LnkTextSumbissionId).HasColumnName("lnkTextSumbissionID");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.SubmissionId).HasColumnName("SubmissionID");

                entity.Property(e => e.TextHeaderId).HasColumnName("TextHeaderID");

                entity.HasOne(d => d.Submission)
                    .WithMany(p => p.LnkTextSubmissions)
                    .HasForeignKey(d => d.SubmissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__lnkTextSu__Submi__5FD33367");

                entity.HasOne(d => d.TextHeader)
                    .WithMany(p => p.LnkTextSubmissions)
                    .HasForeignKey(d => d.TextHeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__lnkTextSu__TextH__5EDF0F2E");
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
                    .HasConstraintName("FK__Publicati__Publi__4336F4B9");

                entity.HasOne(d => d.PublicationType)
                    .WithMany(p => p.Publications)
                    .HasForeignKey(d => d.PublicationTypeId)
                    .HasConstraintName("FK__Publicati__Publi__4242D080");
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

            modelBuilder.Entity<SavedView>(entity =>
            {
                entity.Property(e => e.SavedViewId).HasColumnName("SavedViewID");

                entity.Property(e => e.BinderId).HasColumnName("BinderID");

                entity.Property(e => e.SearchValue)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.SetValue)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SortValue)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.ViewName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Binder)
                    .WithMany(p => p.SavedViews)
                    .HasForeignKey(d => d.BinderId)
                    .HasConstraintName("FK__SavedView__Binde__76818E95");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SavedViews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__SavedView__UserI__62AFA012");
            });

            modelBuilder.Entity<SimpleUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__SimpleUs__1788CCACCF300043");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.AspNetUserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("AspNetUserID");

                entity.Property(e => e.UserName).HasMaxLength(300);

                entity.HasOne(d => d.AspNetUser)
                    .WithMany(p => p.SimpleUsers)
                    .HasForeignKey(d => d.AspNetUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SimpleUse__AspNe__39AD8A7F");
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
                    .HasConstraintName("FK__Submissio__Publi__5A1A5A11");

                entity.HasOne(d => d.SubmissionStatus)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.SubmissionStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Submissio__Submi__5B0E7E4A");

                entity.HasOne(d => d.TextHeader)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.TextHeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Submissio__TextH__5C02A283");
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

                entity.Property(e => e.BinderId).HasColumnName("BinderID");

                entity.Property(e => e.GroupTitle)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.OwnerId).HasColumnName("OwnerID");

                entity.Property(e => e.SavedViewId).HasColumnName("SavedViewID");

                entity.HasOne(d => d.Binder)
                    .WithMany(p => p.TextGroups)
                    .HasForeignKey(d => d.BinderId)
                    .HasConstraintName("FK__TextGroup__Binde__0A888742");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.TextGroups)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK__TextGroup__Owner__49E3F248");

                entity.HasOne(d => d.SavedView)
                    .WithMany(p => p.TextGroups)
                    .HasForeignKey(d => d.SavedViewId)
                    .HasConstraintName("FK__TextGroup__Saved__1CA7377D");
            });

            modelBuilder.Entity<TextHeader>(entity =>
            {
                entity.Property(e => e.TextHeaderId).HasColumnName("TextHeaderID");

                entity.Property(e => e.BinderId).HasColumnName("BinderID");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.LastRead).HasColumnType("datetime");

                entity.Property(e => e.TextId).HasColumnName("TextID");

                entity.Property(e => e.TextRevisionStatusId).HasColumnName("TextRevisionStatusID");

                entity.Property(e => e.Title)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Binder)
                    .WithMany(p => p.TextHeaders)
                    .HasForeignKey(d => d.BinderId)
                    .HasConstraintName("FK__TextHeade__Binde__09946309");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TextHeaderCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__TextHeade__Creat__4EA8A765");

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.TextHeaderLastModifiedByNavigations)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .HasConstraintName("FK__TextHeade__LastM__4F9CCB9E");

                entity.HasOne(d => d.LastReadByNavigation)
                    .WithMany(p => p.TextHeaderLastReadByNavigations)
                    .HasForeignKey(d => d.LastReadBy)
                    .HasConstraintName("FK__TextHeade__LastR__5090EFD7");

                entity.HasOne(d => d.Text)
                    .WithMany(p => p.TextHeaders)
                    .HasForeignKey(d => d.TextId)
                    .HasConstraintName("FK__TextHeade__TextI__4DB4832C");

                entity.HasOne(d => d.TextRevisionStatus)
                    .WithMany(p => p.TextHeaders)
                    .HasForeignKey(d => d.TextRevisionStatusId)
                    .HasConstraintName("FK__TextHeade__TextR__51851410");

                entity.HasOne(d => d.VersionOfNavigation)
                    .WithMany(p => p.InverseVersionOfNavigation)
                    .HasForeignKey(d => d.VersionOf)
                    .HasConstraintName("FK__TextHeade__Versi__52793849");
            });

            modelBuilder.Entity<TextHeaderTitleDefaultType>(entity =>
            {
                entity.Property(e => e.TextHeaderTitleDefaultType1)
                    .HasMaxLength(100)
                    .HasColumnName("TextHeaderTitleDefaultType");
            });

            modelBuilder.Entity<TextRecord>(entity =>
            {
                entity.ToTable("TextRecord");

                entity.Property(e => e.TextRecordId).HasColumnName("TextRecordID");

                entity.Property(e => e.Recorded).HasColumnType("datetime");

                entity.Property(e => e.TextHeaderId).HasColumnName("TextHeaderID");

                entity.Property(e => e.TextId).HasColumnName("TextID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.TextHeader)
                    .WithMany(p => p.TextRecords)
                    .HasForeignKey(d => d.TextHeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TextRecor__TextH__5555A4F4");

                entity.HasOne(d => d.Text)
                    .WithMany(p => p.TextRecords)
                    .HasForeignKey(d => d.TextId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TextRecor__TextI__5649C92D");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TextRecords)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__TextRecor__UserI__573DED66");
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
