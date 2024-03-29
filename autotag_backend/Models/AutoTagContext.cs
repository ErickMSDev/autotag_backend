﻿using System;
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
        public virtual DbSet<AccountRequest> AccountRequests { get; set; } = null!;
        public virtual DbSet<DateDimension> DateDimensions { get; set; } = null!;
        public virtual DbSet<DiscountCode> DiscountCodes { get; set; } = null!;
        public virtual DbSet<DiscountCodeType> DiscountCodeTypes { get; set; } = null!;
        public virtual DbSet<Document> Documents { get; set; } = null!;
        public virtual DbSet<DocumentDetail> DocumentDetails { get; set; } = null!;
        public virtual DbSet<DocumentState> DocumentStates { get; set; } = null!;
        public virtual DbSet<DomainBlacklist> DomainBlacklists { get; set; } = null!;
        public virtual DbSet<Freeway> Freeways { get; set; } = null!;
        public virtual DbSet<Gantry> Gantries { get; set; } = null!;
        public virtual DbSet<GantryDirection> GantryDirections { get; set; } = null!;
        public virtual DbSet<Gateway> Gateways { get; set; } = null!;
        public virtual DbSet<Invoice> Invoices { get; set; } = null!;
        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; } = null!;
        public virtual DbSet<InvoiceState> InvoiceStates { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<NotificationType> NotificationTypes { get; set; } = null!;
        public virtual DbSet<PatentGroup> PatentGroups { get; set; } = null!;
        public virtual DbSet<PaymentCycle> PaymentCycles { get; set; } = null!;
        public virtual DbSet<PeopleTransit> PeopleTransits { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        public virtual DbSet<Portal> Portals { get; set; } = null!;
        public virtual DbSet<PortalAccount> PortalAccounts { get; set; } = null!;
        public virtual DbSet<PortalAccountStatus> PortalAccountStatuses { get; set; } = null!;
        public virtual DbSet<Price> Prices { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;
        public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = null!;
        public virtual DbSet<PurchaseOrderState> PurchaseOrderStates { get; set; } = null!;
        public virtual DbSet<RecoverPassword> RecoverPasswords { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<TransactionState> TransactionStates { get; set; } = null!;
        public virtual DbSet<UnbilledTransit> UnbilledTransits { get; set; } = null!;
        public virtual DbSet<Vehicle> Vehicles { get; set; } = null!;
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

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

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

                entity.Property(e => e.UseDevelopmentPurchaseData).HasColumnName("use_development_purchase_data");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<AccountRequest>(entity =>
            {
                entity.ToTable("AccountRequest");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

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
            });

            modelBuilder.Entity<DateDimension>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DateDimension");

                entity.HasIndex(e => e.TheDate, "IX_THEDATE_INC_THE_FIRST_OF_MONTH");

                entity.Property(e => e.Has53Isoweeks).HasColumnName("Has53ISOWeeks");

                entity.Property(e => e.Mmyyyy)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("MMYYYY")
                    .IsFixedLength();

                entity.Property(e => e.Style101)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Style103)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Style112)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Style120)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TheDate).HasColumnType("date");

                entity.Property(e => e.TheDayName).HasMaxLength(30);

                entity.Property(e => e.TheFirstOfMonth).HasColumnType("date");

                entity.Property(e => e.TheFirstOfNextMonth).HasColumnType("date");

                entity.Property(e => e.TheFirstOfQuarter).HasColumnType("date");

                entity.Property(e => e.TheFirstOfWeek).HasColumnType("date");

                entity.Property(e => e.TheFirstOfYear).HasColumnType("date");

                entity.Property(e => e.TheIsoweek).HasColumnName("TheISOweek");

                entity.Property(e => e.TheIsoyear).HasColumnName("TheISOYear");

                entity.Property(e => e.TheLastOfMonth).HasColumnType("date");

                entity.Property(e => e.TheLastOfNextMonth).HasColumnType("date");

                entity.Property(e => e.TheLastOfQuarter).HasColumnType("date");

                entity.Property(e => e.TheLastOfWeek).HasColumnType("date");

                entity.Property(e => e.TheLastOfYear).HasColumnType("date");

                entity.Property(e => e.TheMonthName).HasMaxLength(30);
            });

            modelBuilder.Entity<DiscountCode>(entity =>
            {
                entity.ToTable("DiscountCode");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.IsRecurring).HasColumnName("is_recurring");

                entity.Property(e => e.MaximumUses).HasColumnName("maximum_uses");

                entity.Property(e => e.OnlyNewCustomers).HasColumnName("only_new_customers");

                entity.Property(e => e.PaymentCycleId).HasColumnName("PaymentCycle_id");

                entity.Property(e => e.ProductId).HasColumnName("Product_id");

                entity.Property(e => e.Value)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("value");
            });

            modelBuilder.Entity<DiscountCodeType>(entity =>
            {
                entity.ToTable("DiscountCodeType");

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

                entity.HasIndex(e => e.DocumentId, "IX_DOCUMENT_INC_ALL");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.Axis)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("axis");

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

                entity.Property(e => e.FreewayId).HasColumnName("Freeway_id");

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

                entity.Property(e => e.TransitedDay)
                    .HasColumnType("date")
                    .HasColumnName("transited_day");

                entity.Property(e => e.TransitedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("transited_on");

                entity.Property(e => e.VehiclePatent)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("vehicle_patent");

                entity.Property(e => e.Velocity).HasColumnName("velocity");

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

            modelBuilder.Entity<DomainBlacklist>(entity =>
            {
                entity.ToTable("DomainBlacklist");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DomainName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("domain_name");
            });

            modelBuilder.Entity<Freeway>(entity =>
            {
                entity.ToTable("Freeway");

                entity.HasIndex(e => e.Code, "IX_Freeway")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.ExternalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("external_code");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.PortalId).HasColumnName("Portal_id");

                entity.HasOne(d => d.Portal)
                    .WithMany(p => p.Freeways)
                    .HasForeignKey(d => d.PortalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Freeway_Portal");
            });

            modelBuilder.Entity<Gantry>(entity =>
            {
                entity.ToTable("Gantry");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.FreewayId).HasColumnName("Freeway_id");

                entity.Property(e => e.GantryDirectionId).HasColumnName("GantryDirection_id");

                entity.Property(e => e.Lat)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("lat");

                entity.Property(e => e.Lon)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("lon");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PrevCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("prev_code");

                entity.Property(e => e.PrevDistance).HasColumnName("prev_distance");

                entity.HasOne(d => d.Freeway)
                    .WithMany(p => p.Gantries)
                    .HasForeignKey(d => d.FreewayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Gantry_Freeway");

                entity.HasOne(d => d.GantryDirection)
                    .WithMany(p => p.Gantries)
                    .HasForeignKey(d => d.GantryDirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Gantry_GantryDirection");
            });

            modelBuilder.Entity<GantryDirection>(entity =>
            {
                entity.ToTable("GantryDirection");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Gateway>(entity =>
            {
                entity.ToTable("Gateway");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApiKeyDev)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("api_key_dev");

                entity.Property(e => e.ApiKeyProd)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("api_key_prod");

                entity.Property(e => e.BackendUrlReturn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("backend_url_return");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.DebtCollectorIdDev)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("debt_collector_id_dev");

                entity.Property(e => e.DebtCollectorIdProd)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("debt_collector_id_prod");

                entity.Property(e => e.Enabled).HasColumnName("enabled");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");

                entity.Property(e => e.RestApiUrlDev)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("rest_api_url_dev");

                entity.Property(e => e.RestApiUrlProd)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("rest_api_url_prod");

                entity.Property(e => e.SecretKeyDev)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("secret_key_dev");

                entity.Property(e => e.SecretKeyProd)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("secret_key_prod");

                entity.Property(e => e.UrlConfirmation)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("url_confirmation");

                entity.Property(e => e.UrlReturn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("url_return");

                entity.Property(e => e.UseDevelopmentData).HasColumnName("use_development_data");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoice");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

                entity.Property(e => e.DueDate)
                    .HasColumnType("datetime")
                    .HasColumnName("due_date");

                entity.Property(e => e.InvoiceStateId).HasColumnName("InvoiceState_id");

                entity.Property(e => e.PurchaseOrderId).HasColumnName("PurchaseOrder_id");

                entity.HasOne(d => d.InvoiceState)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.InvoiceStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoice_InvoiceState");

                entity.HasOne(d => d.PurchaseOrder)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoice_PurchaseOrder");
            });

            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.ToTable("InvoiceDetail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.InvoiceId).HasColumnName("Invoice_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceDetails)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceDetail_Invoice");
            });

            modelBuilder.Entity<InvoiceState>(entity =>
            {
                entity.ToTable("InvoiceState");

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

            modelBuilder.Entity<PatentGroup>(entity =>
            {
                entity.ToTable("PatentGroup");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LatestPatentDownloaded)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("latest_patent_downloaded");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PatentDigits)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("patent_digits");
            });

            modelBuilder.Entity<PaymentCycle>(entity =>
            {
                entity.ToTable("PaymentCycle");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Represents)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("represents");
            });

            modelBuilder.Entity<PeopleTransit>(entity =>
            {
                entity.ToTable("PeopleTransit");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EndTransit)
                    .HasColumnType("datetime")
                    .HasColumnName("end_transit");

                entity.Property(e => e.PersonId).HasColumnName("Person_id");

                entity.Property(e => e.StartTransit)
                    .HasColumnType("datetime")
                    .HasColumnName("start_transit");

                entity.Property(e => e.VehiclePatent)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("vehicle_patent");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PeopleTransits)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PeopleTransit_Person");
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

                entity.Property(e => e.ErrorMailAlreadySent).HasColumnName("error_mail_already_sent");

                entity.Property(e => e.ErrorMessage)
                    .IsUnicode(false)
                    .HasColumnName("error_message");

                entity.Property(e => e.HasFirstSuccessfulProcess).HasColumnName("has_first_successful_process");

                entity.Property(e => e.HasPendingProcess).HasColumnName("has_pending_process");

                entity.Property(e => e.LastProcessRequestDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_process_request_date");

                entity.Property(e => e.LastSuccessfulProcessDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_successful_process_date");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.PortalAccountStatusId).HasColumnName("PortalAccountStatus_id");

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

                entity.HasOne(d => d.PortalAccountStatus)
                    .WithMany(p => p.PortalAccounts)
                    .HasForeignKey(d => d.PortalAccountStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PortalAccount_PortalAccountStatus");

                entity.HasOne(d => d.Portal)
                    .WithMany(p => p.PortalAccounts)
                    .HasForeignKey(d => d.PortalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PortalAcc__Porta__6EF57B66");
            });

            modelBuilder.Entity<PortalAccountStatus>(entity =>
            {
                entity.ToTable("PortalAccountStatus");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Price>(entity =>
            {
                entity.ToTable("Price");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.DiscountText)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("discount_text");

                entity.Property(e => e.Enabled).HasColumnName("enabled");

                entity.Property(e => e.PaymentCycleId).HasColumnName("PaymentCycle_id");

                entity.Property(e => e.ProductId).HasColumnName("Product_id");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Enabled).HasColumnName("enabled");

                entity.Property(e => e.IsComplement).HasColumnName("is_complement");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.ParentProductId).HasColumnName("parent_product_id");
            });

            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.ToTable("PurchaseOrder");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("Account_id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.AmountWithoutDiscount)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("amount_without_discount");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

                entity.Property(e => e.DiscountCodeId).HasColumnName("DiscountCode_id");

                entity.Property(e => e.NextDueDate)
                    .HasColumnType("datetime")
                    .HasColumnName("next_due_date");

                entity.Property(e => e.PurchaseOrderStateId).HasColumnName("PurchaseOrderState_id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.PurchaseOrders)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseOrder_Account");

                entity.HasOne(d => d.DiscountCode)
                    .WithMany(p => p.PurchaseOrders)
                    .HasForeignKey(d => d.DiscountCodeId)
                    .HasConstraintName("FK_PurchaseOrder_DiscountCode");
            });

            modelBuilder.Entity<PurchaseOrderDetail>(entity =>
            {
                entity.ToTable("PurchaseOrderDetail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.PaymentCycleId).HasColumnName("PaymentCycle_id");

                entity.Property(e => e.ProductId).HasColumnName("Product_id");

                entity.Property(e => e.PurchaseOrderId).HasColumnName("PurchaseOrder_id");
            });

            modelBuilder.Entity<PurchaseOrderState>(entity =>
            {
                entity.ToTable("PurchaseOrderState");

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

            modelBuilder.Entity<RecoverPassword>(entity =>
            {
                entity.ToTable("RecoverPassword");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");
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

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

                entity.Property(e => e.GatewayId).HasColumnName("Gateway_id");

                entity.Property(e => e.GatewayOrder).HasColumnName("gateway_order");

                entity.Property(e => e.GatewayPaymentMethod).HasColumnName("gateway_payment_method");

                entity.Property(e => e.GatewayToken)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("gateway_token");

                entity.Property(e => e.InvoiceId).HasColumnName("Invoice_id");

                entity.Property(e => e.IsDevelopment).HasColumnName("is_development");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("payment_date");

                entity.Property(e => e.TransactionStateId).HasColumnName("TransactionState_id");

                entity.HasOne(d => d.Gateway)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.GatewayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Gateway");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Invoice");

                entity.HasOne(d => d.TransactionState)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.TransactionStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_TransactionState");
            });

            modelBuilder.Entity<TransactionState>(entity =>
            {
                entity.ToTable("TransactionState");

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

                entity.Property(e => e.FreewayId).HasColumnName("Freeway_id");

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

                entity.Property(e => e.TransitedDay)
                    .HasColumnType("date")
                    .HasColumnName("transited_day");

                entity.Property(e => e.TransitedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("transited_on");

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

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("Vehicle");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Brand)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("brand");

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("color");

                entity.Property(e => e.DownloadedPatent)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("downloaded_patent");

                entity.Property(e => e.Fines)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("fines");

                entity.Property(e => e.Model)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("model");

                entity.Property(e => e.NChasis)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("n_chasis");

                entity.Property(e => e.NMotor)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("n_motor");

                entity.Property(e => e.OwnerName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("owner_name");

                entity.Property(e => e.OwnerRun)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("owner_run");

                entity.Property(e => e.Patent)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("patent");

                entity.Property(e => e.PatentGroupId).HasColumnName("PatentGroup_id");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("type");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.PatentGroup)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.PatentGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vehicle_PatentGroup");
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
