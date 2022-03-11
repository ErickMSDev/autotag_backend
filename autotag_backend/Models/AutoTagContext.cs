using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AutoTagBackEnd.Models
{
    public partial class AutoTagContext : DbContext
    {
        public AutoTagContext()
        {
        }

        public AutoTagContext(DbContextOptions<AutoTagContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Document> Documents { get; set; } = null!;
        public virtual DbSet<DocumentDetail> DocumentDetails { get; set; } = null!;
        public virtual DbSet<DocumentState> DocumentStates { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<NotificationType> NotificationTypes { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        public virtual DbSet<Portal> Portals { get; set; } = null!;
        public virtual DbSet<PortalAccount> PortalAccounts { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<UnbilledTransit> UnbilledTransits { get; set; } = null!;
        public virtual DbSet<VehicleAssignment> VehicleAssignments { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=Production");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.HasIndex(e => e.Email, "Account_UN_MAIL")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Enabled).HasColumnName("enabled");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("last_name");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("Role_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("Document");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.DocumentStateId).HasColumnName("DocumentState_id");

                entity.Property(e => e.DownloadDate)
                    .HasColumnType("datetime")
                    .HasColumnName("download_date");

                entity.Property(e => e.Downloaded).HasColumnName("downloaded");

                entity.Property(e => e.DteName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("dte_name");

                entity.Property(e => e.ExpirationDate)
                    .HasColumnType("date")
                    .HasColumnName("expiration_date");

                entity.Property(e => e.IssueDate)
                    .HasColumnType("date")
                    .HasColumnName("issue_date");

                entity.Property(e => e.PeriodEndDate)
                    .HasColumnType("date")
                    .HasColumnName("period_end_date");

                entity.Property(e => e.PeriodStartDate)
                    .HasColumnType("date")
                    .HasColumnName("period_start_date");

                entity.Property(e => e.PortalAccountId).HasColumnName("PortalAccount_id");

                entity.HasOne(d => d.DocumentState)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.DocumentStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Document_DocumentState");

                entity.HasOne(d => d.PortalAccount)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.PortalAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Document_PortalAccount_id_fk");
            });

            modelBuilder.Entity<DocumentDetail>(entity =>
            {
                entity.ToTable("DocumentDetail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.Axis)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("axis");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.DayType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("day_type");

                entity.Property(e => e.Dealership)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("dealership");

                entity.Property(e => e.Direction)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("direction");

                entity.Property(e => e.DocumentCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Document_code");

                entity.Property(e => e.DocumentId).HasColumnName("Document_id");

                entity.Property(e => e.Gantry)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("gantry");

                entity.Property(e => e.Kilometres)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("kilometres");

                entity.Property(e => e.Place)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("place");

                entity.Property(e => e.RateType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("rate_type");

                entity.Property(e => e.Tag)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tag");

                entity.Property(e => e.VehiclePatent)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("vehicle_patent");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentDetails)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DocumentDetail_Document_id_fk");
            });

            modelBuilder.Entity<DocumentState>(entity =>
            {
                entity.ToTable("DocumentState");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("Account_id");

                entity.Property(e => e.Clicked).HasColumnName("clicked");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Link)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("link");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.Text)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("text");

                entity.Property(e => e.TypeId).HasColumnName("Type_id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_Account");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_NotificationType");
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.ToTable("NotificationType");

                entity.HasIndex(e => e.Code, "IX_NotificationType")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");

                entity.HasComment("Son las personas encargadas de los vehiculos");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("Account_id");

                entity.Property(e => e.Enabled).HasColumnName("enabled");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("last_name");

                entity.Property(e => e.Mail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("mail");

                entity.Property(e => e.Run)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("run");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.People)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Person_Account");
            });

            modelBuilder.Entity<Portal>(entity =>
            {
                entity.ToTable("Portal");

                entity.HasIndex(e => e.Code, "UQ__Portal__357D4CF9AC3FFCB3")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Order).HasColumnName("order");
            });

            modelBuilder.Entity<PortalAccount>(entity =>
            {
                entity.ToTable("PortalAccount");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("Account_id");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

                entity.Property(e => e.DeletionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("deletion_date");

                entity.Property(e => e.Enabled).HasColumnName("enabled");

                entity.Property(e => e.ErrorMessage)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("error_message");

                entity.Property(e => e.HasError).HasColumnName("has_error");

                entity.Property(e => e.HasPendingProcess).HasColumnName("has_pending_process");

                entity.Property(e => e.IsBeingProcessed).HasColumnName("is_being_processed");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.PortalId).HasColumnName("Portal_id");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("row_version");

                entity.Property(e => e.Run)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("run");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.PortalAccounts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PortalAcc__Accou__6FE99F9F");

                entity.HasOne(d => d.Portal)
                    .WithMany(p => p.PortalAccounts)
                    .HasForeignKey(d => d.PortalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PortalAcc__Porta__6EF57B66");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.HasIndex(e => e.Code, "IX_Role")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<UnbilledTransit>(entity =>
            {
                entity.ToTable("UnbilledTransit");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.Axis)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("axis");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.DayType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("day_type");

                entity.Property(e => e.Dealership)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("dealership");

                entity.Property(e => e.Direction)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("direction");

                entity.Property(e => e.Gantry)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("gantry");

                entity.Property(e => e.Kilometres)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("kilometres");

                entity.Property(e => e.Place)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("place");

                entity.Property(e => e.PortalAccountId).HasColumnName("PortalAccount_id");

                entity.Property(e => e.RateType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("rate_type");

                entity.Property(e => e.Tag)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tag");

                entity.Property(e => e.VehiclePatent)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("vehicle_patent");

                entity.HasOne(d => d.PortalAccount)
                    .WithMany(p => p.UnbilledTransits)
                    .HasForeignKey(d => d.PortalAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnbilledTransit_PortalAccount");
            });

            modelBuilder.Entity<VehicleAssignment>(entity =>
            {
                entity.ToTable("VehicleAssignment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AssignmentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("assignment_date");

                entity.Property(e => e.PersonId).HasColumnName("Person_id");

                entity.Property(e => e.VehiclePatent)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("vehicle_patent");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.VehicleAssignments)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_VehicleAssignment_Person");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
