using System.ComponentModel.DataAnnotations;

namespace DuLichNhaTrang.Models.ViewModels
{
    // Dữ liệu người dùng nhập ở trang Đăng bài
    public class PostCreateViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        [StringLength(200, ErrorMessage = "Tiêu đề tối đa 200 ký tự")]
        [Display(Name = "Tiêu đề")]
        public string TieuDe { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        [Display(Name = "Mô tả")]
        public string MoTa { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng chọn ảnh địa điểm")]
        [Display(Name = "Hình ảnh")]
        public IFormFile? HinhAnh { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại địa điểm")]
        [Display(Name = "Loại địa điểm")]
        public int CategoryId { get; set; }

        // Vị trí trên bản đồ: dạng "10.7769,106.7009". Để trống được.
        [StringLength(50)]
        [Display(Name = "Vị trí trên bản đồ (không bắt buộc)")]
        public string? ToaDo { get; set; }
    }
}
