using System.ComponentModel.DataAnnotations;

namespace DuLichNhaTrang.Models.ViewModels
{
    // Bước 2: nhập mã đặt lại và mật khẩu mới
    public class DatLaiMatKhauViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập mã đặt lại mật khẩu")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Mã đặt lại gồm 6 chữ số")]
        [Display(Name = "Mã đặt lại (6 số)")]
        public string MaDatLai { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 50 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string MatKhauMoi { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu mới")]
        [DataType(DataType.Password)]
        [Compare("MatKhauMoi", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        [Display(Name = "Nhập lại mật khẩu mới")]
        public string NhapLaiMatKhau { get; set; } = "";
    }
}
