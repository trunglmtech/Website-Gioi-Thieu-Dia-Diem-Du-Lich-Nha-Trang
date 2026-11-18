using System.ComponentModel.DataAnnotations;

namespace DuLichNhaTrang.Models.ViewModels
{
    // Dữ liệu người dùng nhập ở trang donate (MÔ PHỎNG, không thanh toán thật)
    public class DonateViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn bài viết muốn donate")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn bài viết muốn donate")]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số tiền muốn donate")]
        [Range(1000, 100000000, ErrorMessage = "Số tiền phải từ 1.000 đến 100.000.000 đồng")]
        [Display(Name = "Số tiền (đồng)")]
        public decimal? SoTien { get; set; }

        // Danh sách bài để đổ vào ô chọn (Controller nạp vào, không phải người dùng nhập)
        public List<Post> DanhSachBai { get; set; } = new List<Post>();
    }
}
