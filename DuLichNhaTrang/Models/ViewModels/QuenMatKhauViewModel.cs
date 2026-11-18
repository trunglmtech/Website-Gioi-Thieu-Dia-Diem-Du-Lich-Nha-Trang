using System.ComponentModel.DataAnnotations;

namespace DuLichNhaTrang.Models.ViewModels
{
    // Bước 1: nhập tên đăng nhập để xin mã đặt lại mật khẩu
    public class QuenMatKhauViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = "";
    }
}
