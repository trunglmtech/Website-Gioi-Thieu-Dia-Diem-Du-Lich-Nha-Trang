using DuLichNhaTrang.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DuLichNhaTrang.Data
{
    // Lớp tạo dữ liệu mẫu. Chạy 1 lần lúc khởi động, chỉ thêm khi CSDL đang trống.
    public static class SeedData
    {
        public static void KhoiTao(AppDbContext context)
        {
            // Nếu máy mới chưa có CSDL thì tự tạo bảng từ migration
            context.Database.Migrate();

            // Đã có người dùng nghĩa là đã có dữ liệu mẫu -> không thêm nữa
            if (context.Users.Any())
            {
                return;
            }

            var hasher = new PasswordHasher<User>();

            // 2 tài khoản mẫu: 1 Admin, 1 Thành viên. Mật khẩu đều là 123456 (đã hash)
            var admin = new User { TenDangNhap = "admin", VaiTro = "Admin" };
            admin.MatKhau = hasher.HashPassword(admin, "123456");

            var member = new User { TenDangNhap = "trung", VaiTro = "Member" };
            member.MatKhau = hasher.HashPassword(member, "123456");

            context.Users.AddRange(admin, member);
            context.SaveChanges();

            // 4 loại địa điểm
            var baiBien = new Category { TenLoai = "Bãi biển" };
            var diTich = new Category { TenLoai = "Di tích - Chùa" };
            var amThuc = new Category { TenLoai = "Ẩm thực" };
            var khuVuiChoi = new Category { TenLoai = "Khu vui chơi" };

            context.Categories.AddRange(baiBien, diTich, amThuc, khuVuiChoi);
            context.SaveChanges();

            // 6 bài mẫu: 5 bài Đã duyệt, 1 bài Chờ duyệt (để thử chức năng duyệt sau)
            var bai1 = new Post
            {
                TieuDe = "Bãi biển Nha Trang",
                MoTa = "Bãi biển chạy dài dọc đường Trần Phú, nước xanh, cát trắng, rất thuận tiện để tắm biển và ngắm bình minh.",
                HinhAnh = "images/sample/bai-bien.jpg",
                TrangThai = "DaDuyet",
                NgayDang = DateTime.Now.AddDays(-12),
                UserId = member.Id,
                CategoryId = baiBien.Id
            };

            var bai2 = new Post
            {
                TieuDe = "Vinpearl Hòn Tre",
                MoTa = "Khu vui chơi giải trí trên đảo Hòn Tre, có cáp treo vượt biển, công viên nước và thủy cung.",
                HinhAnh = "images/sample/vinpearl.jpg",
                TrangThai = "DaDuyet",
                NgayDang = DateTime.Now.AddDays(-10),
                // Bài này đã được donate nên còn hạn ưu tiên 5 ngày nữa
                FeaturedUntil = DateTime.Now.AddDays(5),
                UserId = member.Id,
                CategoryId = khuVuiChoi.Id
            };

            var bai3 = new Post
            {
                TieuDe = "Tháp Bà Ponagar",
                MoTa = "Di tích Chăm cổ nằm trên đồi Cù Lao, kiến trúc đẹp, là điểm tham quan nổi tiếng của Nha Trang.",
                HinhAnh = "images/sample/thap-ba.jpg",
                TrangThai = "DaDuyet",
                NgayDang = DateTime.Now.AddDays(-7),
                UserId = admin.Id,
                CategoryId = diTich.Id
            };

            var bai4 = new Post
            {
                TieuDe = "Chợ đêm Nha Trang",
                MoTa = "Chợ đêm gần bến tàu, bán hải sản, đồ nướng và quà lưu niệm, mở từ chiều tối đến khuya.",
                HinhAnh = "images/sample/cho-dem.jpg",
                TrangThai = "DaDuyet",
                NgayDang = DateTime.Now.AddDays(-5),
                UserId = member.Id,
                CategoryId = amThuc.Id
            };

            var bai5 = new Post
            {
                TieuDe = "Bún chả cá Nha Trang",
                MoTa = "Món ăn sáng đặc trưng của người Nha Trang, nước dùng ngọt từ cá tươi, ăn kèm rau sống.",
                HinhAnh = "images/sample/bun-cha-ca.jpg",
                TrangThai = "DaDuyet",
                NgayDang = DateTime.Now.AddDays(-3),
                UserId = member.Id,
                CategoryId = amThuc.Id
            };

            var bai6 = new Post
            {
                TieuDe = "Nhà thờ Núi",
                MoTa = "Nhà thờ Công giáo xây trên đồi nhỏ giữa thành phố, kiến trúc Gothic, nhìn thấy từ nhiều nơi.",
                HinhAnh = "images/sample/nha-tho-nui.jpg",
                // Bài này đang chờ Admin duyệt
                TrangThai = "ChoDuyet",
                NgayDang = DateTime.Now.AddDays(-1),
                UserId = member.Id,
                CategoryId = diTich.Id
            };

            context.Posts.AddRange(bai1, bai2, bai3, bai4, bai5, bai6);
            context.SaveChanges();

            // 1 lượt donate mẫu cho bài Vinpearl (50.000 đồng)
            context.Donations.Add(new Donation
            {
                PostId = bai2.Id,
                UserId = member.Id,
                SoTien = 50000,
                NgayDonate = DateTime.Now.AddDays(-2)
            });
            context.SaveChanges();
        }
    }
}
