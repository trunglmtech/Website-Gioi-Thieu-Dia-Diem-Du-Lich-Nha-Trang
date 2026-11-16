using System.ComponentModel.DataAnnotations;

namespace DuLichNhaTrang.Models
{
    // Bảng Posts: bài giới thiệu địa điểm du lịch
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        [StringLength(200, ErrorMessage = "Tiêu đề tối đa 200 ký tự")]
        public string TieuDe { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string MoTa { get; set; } = "";

        // Chỉ lưu TÊN FILE ảnh (ví dụ: bai-bien.jpg), file thật nằm trong wwwroot/images
        [StringLength(255)]
        public string? HinhAnh { get; set; }

        // Chỉ nhận 3 giá trị: "ChoDuyet", "DaDuyet", "TuChoi"
        [Required]
        [StringLength(20)]
        public string TrangThai { get; set; } = "ChoDuyet";

        public DateTime NgayDang { get; set; } = DateTime.Now;

        // Hạn được ưu tiên lên đầu trang. NULL = bài chưa từng donate
        public DateTime? FeaturedUntil { get; set; }

        // Khóa ngoại: bài do ai đăng
        public int UserId { get; set; }
        public User? User { get; set; }

        // Khóa ngoại: bài thuộc loại địa điểm nào
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Một bài có nhiều lượt donate
        public List<Donation> Donations { get; set; } = new List<Donation>();
    }
}
