using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuLichNhaTrang.Models
{
    // Bảng PasswordResets: mã đặt lại mật khẩu (mô phỏng việc gửi mã về email)
    public class PasswordReset
    {
        public int Id { get; set; }

        // Mã 6 số để thành viên nhập lại khi đặt mật khẩu mới
        [StringLength(6)]
        public string MaDatLai { get; set; } = "";

        // Mã chỉ dùng được trong 15 phút
        [Column(TypeName = "datetime")]
        public DateTime HetHan { get; set; }

        // Mã chỉ dùng được 1 lần
        public bool DaDung { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime NgayYeuCau { get; set; } = DateTime.Now;

        // Khóa ngoại: mã này tạo cho tài khoản nào
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
