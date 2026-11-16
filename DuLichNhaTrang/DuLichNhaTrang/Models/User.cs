using System.ComponentModel.DataAnnotations;

namespace DuLichNhaTrang.Models
{
    // Bảng Users: lưu tài khoản người dùng (Admin và Member)
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập tối đa 50 ký tự")]
        public string TenDangNhap { get; set; } = "";

        // Không bao giờ lưu mật khẩu gốc, chỉ lưu mật khẩu ĐÃ HASH
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(255)]
        public string MatKhau { get; set; } = "";

        // Chỉ nhận 2 giá trị: "Admin" hoặc "Member"
        [Required]
        [StringLength(20)]
        public string VaiTro { get; set; } = "Member";

        // Một người dùng có nhiều bài viết
        public List<Post> Posts { get; set; } = new List<Post>();

        // Một người dùng có nhiều lượt donate
        public List<Donation> Donations { get; set; } = new List<Donation>();
    }
}
