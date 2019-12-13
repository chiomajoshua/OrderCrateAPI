using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OrderCrateAPI.Entities
{
    public partial class OrdercratedbContext : DbContext
    {
        public virtual DbSet<Business> Business { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Delivery> Delivery { get; set; }
        public virtual DbSet<Login> Login { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<User> User { get; set; }

        public OrdercratedbContext(DbContextOptions<OrdercratedbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=./;Initial Catalog=OrderCrateDB;User ID=sa;Password=24680");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Business>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Business)
                    .HasForeignKey(d => d.UserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Business_User");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Platform).IsUnicode(false);

                entity.Property(e => e.Title).IsUnicode(false);

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.BusinessID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_Business");
            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.HasIndex(e => e.OrderID)
                    .HasName("IX_Delivery")
                    .IsUnique();

                entity.Property(e => e.Status).IsUnicode(false);

                entity.Property(e => e.Type).IsUnicode(false);

                entity.Property(e => e.Vendor).IsUnicode(false);

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.Delivery)
                    .HasForeignKey(d => d.BusinessID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Delivery_Business");

                entity.HasOne(d => d.Order)
                    .WithOne(p => p.Delivery)
                    .HasForeignKey<Delivery>(d => d.OrderID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Delivery_Order");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.HasIndex(e => e.UserID)
                    .HasName("IX_Login")
                    .IsUnique();

                entity.Property(e => e.Username).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Login)
                    .HasForeignKey<Login>(d => d.UserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Login_User");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.OrderPlatform).IsUnicode(false);

                entity.Property(e => e.Status).IsUnicode(false);

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.BusinessID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Business");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.CustomerID)
                    .HasConstraintName("FK_Order_Customer");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasIndex(e => e.OrderID)
                    .HasName("IX_Payment")
                    .IsUnique();

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.BusinessID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_Business");

                entity.HasOne(d => d.Order)
                    .WithOne(p => p.Payment)
                    .HasForeignKey<Payment>(d => d.OrderID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_Order");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.DebitCredit).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.BusinessID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Business");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Firstname).IsUnicode(false);

                entity.Property(e => e.Gender).IsUnicode(false);

                entity.Property(e => e.Lastname).IsUnicode(false);
            });

            OnModelCreatingExt(modelBuilder);
        }

        partial void OnModelCreatingExt(ModelBuilder modelBuilder);
    }
}