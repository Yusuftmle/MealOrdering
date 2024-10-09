using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MealOrdering.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MealOrdering.Server.Data.Context
{
     public class MealOrderingDbContext:DbContext
    {
        public MealOrderingDbContext(DbContextOptions<MealOrderingDbContext> options) : base(options) { }
        
          public virtual DbSet<Users> Users { get; set; }
          public virtual DbSet<Orders> Orders { get; set; }
          public virtual DbSet<OrderItems> OrderItems { get; set; }
          public virtual DbSet<Suppliers> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users", "meal");

                entity.HasKey(e => e.Id).HasName("pk_user_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("uniqueidentifier")
                    .HasDefaultValueSql("NEWID()")
                    .IsRequired();

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasColumnType("nvarchar")
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasColumnType("nvarchar")
                    .HasMaxLength(100);

                entity.Property(e => e.EmailAdress)
                    .HasColumnName("email_address")
                    .HasColumnType("nvarchar")
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("nvarchar")
                    .HasMaxLength(250);

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isactive")
                    .HasColumnType("bit");
            });

            modelBuilder.Entity<Suppliers>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("pk_supplier_id");

                entity.ToTable("suppliers", "meal");

                entity.Property(e => e.Id).HasColumnName("id").HasColumnType("uniqueidentifier").HasDefaultValueSql("NEWID()").IsRequired();

                entity.Property(e => e.IsActive).HasColumnName("isactive").HasColumnType("bit");
                entity.Property(e => e.Name).HasColumnName("name").HasColumnType("nvarchar").HasMaxLength(100);
                entity.Property(e => e.CreateDate).HasColumnName("createdate").HasColumnType("datetime").HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

                entity.Property(e => e.WebUrl).HasColumnName("web_url").HasColumnType("nvarchar").HasMaxLength(500);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("pk_order_id");

                entity.ToTable("orders", "meal");

                entity.Property(e => e.Id).HasColumnName("id").HasColumnType("uniqueidentifier").HasDefaultValueSql("NEWID()");
                entity.Property(e => e.Name).HasColumnName("name").HasColumnType("nvarchar").HasMaxLength(100);
                entity.Property(e => e.Description).HasColumnName("description").HasColumnType("nvarchar").HasMaxLength(1000);

                entity.Property(e => e.CreateDate).HasColumnName("createdate").HasColumnType("datetime").HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
                entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id").HasColumnType("uniqueidentifier");
                entity.Property(e => e.SupplierId).HasColumnName("supplier_id").HasColumnType("uniqueidentifier").IsRequired().ValueGeneratedNever();
                entity.Property(e => e.ExpireDate).HasColumnName("expire_date").HasColumnType("datetime").IsRequired();

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("fk_user_order_id")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("fk_supplier_order_id")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItems>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("pk_orderItem_id");

                entity.ToTable("order_items", "meal");

                entity.Property(e => e.Id).HasColumnName("id").HasColumnType("uniqueidentifier").HasDefaultValueSql("NEWID()");
                entity.Property(e => e.Description).HasColumnName("description").HasColumnType("nvarchar").HasMaxLength(1000);
                entity.Property(e => e.CreateDate).HasColumnName("createdate").HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id").HasColumnType("uniqueidentifier");
                entity.Property(e => e.OrderId).HasColumnName("order_id").HasColumnType("uniqueidentifier");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("fk_orderitems_order_id")
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.CreatedOrderItems)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("fk_orderitems_user_id")
                    .OnDelete(DeleteBehavior.NoAction);
            });








            base.OnModelCreating(modelBuilder);
        }
    }
}
