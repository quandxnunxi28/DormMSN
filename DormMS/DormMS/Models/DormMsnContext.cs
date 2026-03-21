using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DormMS.Models;

public partial class DormMsnContext : DbContext
{
    public DormMsnContext()
    {
    }

    public DormMsnContext(DbContextOptions<DormMsnContext> options)
        : base(options)
    {
    }

    public virtual DbSet<News> News { get; set; }
    public virtual DbSet<HostelUser> HostelUsers { get; set; }
    public virtual DbSet<Hostel> Hostels { get; set; }
    public virtual DbSet<Room> Rooms { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Allotment> Allotments { get; set; }
    public virtual DbSet<Complaint> Complaints { get; set; }
    public virtual DbSet<Inventory> Inventories { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Visitor> Visitors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=localhost;database=DormMSN;Integrated Security=SSPI;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hostel>(entity =>
        {
            entity.ToTable("hostels");

            entity.HasKey(e => e.HostelId);

            entity.Property(e => e.HostelId).HasColumnName("hostel_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Location).HasColumnName("location");
            entity.Property(e => e.TotalRooms).HasColumnName("total_rooms");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.ToTable("rooms");

            entity.HasKey(e => e.RoomId);

            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.HostelId).HasColumnName("hostel_id");
            entity.Property(e => e.RoomNumber).HasColumnName("room_number");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Occupied).HasColumnName("occupied");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne<Hostel>()
                .WithMany()
                .HasForeignKey(e => e.HostelId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");

            entity.HasKey(e => e.RoleId);

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
        });
        modelBuilder.Entity<HostelUser>(entity =>
        {
            entity.ToTable("hostel_user");

            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Username).HasColumnName("username");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Course).HasColumnName("course");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.HostelId).HasColumnName("hostel_id");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne<Role>()
                .WithMany()
                .HasForeignKey(e => e.Role);

            entity.HasOne<Hostel>()
                .WithMany()
                .HasForeignKey(e => e.HostelId);
        });

        modelBuilder.Entity<Allotment>(entity =>
        {
            entity.ToTable("allotments");

            entity.HasKey(e => e.AllotmentId);

            entity.Property(e => e.AllotmentId).HasColumnName("allotment_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.AllotDate).HasColumnName("allot_date");
            entity.Property(e => e.LeaveDate).HasColumnName("leave_date");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne<HostelUser>()
                .WithMany()
                .HasForeignKey(e => e.UserId);

            entity.HasOne<Room>()
                .WithMany()
                .HasForeignKey(e => e.RoomId);
        });
        modelBuilder.Entity<Complaint>(entity =>
        {
            entity.ToTable("complaints");

            entity.HasKey(e => e.ComplaintId);

            entity.Property(e => e.ComplaintId).HasColumnName("complaint_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.Issue).HasColumnName("issue");
            entity.Property(e => e.DateFiled).HasColumnName("date_filed");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne<HostelUser>()
                .WithMany()
                .HasForeignKey(e => e.UserId);

            entity.HasOne<Room>()
                .WithMany()
                .HasForeignKey(e => e.RoomId);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.ToTable("inventory");

            entity.HasKey(e => e.ItemId);

            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.HostelId).HasColumnName("hostel_id");
            entity.Property(e => e.ItemName).HasColumnName("item_name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Condition).HasColumnName("condition");

            entity.HasOne<Hostel>()
                .WithMany()
                .HasForeignKey(e => e.HostelId);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("payment");

            entity.HasKey(e => e.PaymentId);

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Method).HasColumnName("method");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne<HostelUser>()
                .WithMany()
                .HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.ToTable("visitors");

            entity.HasKey(e => e.VisitorId);

            entity.Property(e => e.VisitorId).HasColumnName("visitor_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Relation).HasColumnName("relation");
            entity.Property(e => e.VisitDate).HasColumnName("visit_date");
            entity.Property(e => e.InTime).HasColumnName("in_time");
            entity.Property(e => e.OutTime).HasColumnName("out_time");

            entity.HasOne<HostelUser>()
                .WithMany()
                .HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.ToTable("news");

            entity.HasKey(e => e.NewsId);

            entity.Property(e => e.NewsId).HasColumnName("news_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Summary).HasColumnName("summary");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.IsImportant).HasColumnName("is_important");

            entity.HasOne<HostelUser>()
                .WithMany()
                .HasForeignKey(e => e.CreatedBy);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
