using DuLichNhaTrang.Models;
using Microsoft.EntityFrameworkCore;

namespace DuLichNhaTrang.Data
{
    // AppDbContext = "cầu nối" giữa code C# và cơ sở dữ liệu SQL Server
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Mỗi DbSet tương ứng với MỘT bảng trong cơ sở dữ liệu
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1) Tên đăng nhập không được trùng nhau
            modelBuilder.Entity<User>()
                .HasIndex(u => u.TenDangNhap)
                .IsUnique();

            // 2) Một User có nhiều Post. Không cho xóa User nếu còn bài viết
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3) Một Category có nhiều Post. Không cho xóa loại địa điểm nếu còn bài
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // 4) Một Post có nhiều Donation. Xóa bài thì xóa luôn donate của bài đó
            modelBuilder.Entity<Donation>()
                .HasOne(d => d.Post)
                .WithMany(p => p.Donations)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // 5) Một User có nhiều Donation. Không cho xóa User nếu còn lượt donate
            modelBuilder.Entity<Donation>()
                .HasOne(d => d.User)
                .WithMany(u => u.Donations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 6) Một User có nhiều mã đặt lại mật khẩu. Xóa tài khoản thì xóa luôn mã
            modelBuilder.Entity<PasswordReset>()
                .HasOne(r => r.User)
                .WithMany(u => u.PasswordResets)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
