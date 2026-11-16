using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuLichNhaTrang.Models
{
    // Bảng Donations: lịch sử donate (MÔ PHỎNG, không thanh toán thật)
    public class Donation
    {
        public int Id { get; set; }

        // Số tiền donate (đồng), không lấy phần thập phân
        [Column(TypeName = "decimal(18,0)")]
        [Range(1000, 100000000, ErrorMessage = "Số tiền phải từ 1.000 đến 100.000.000 đồng")]
        public decimal SoTien { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime NgayDonate { get; set; } = DateTime.Now;

        // Khóa ngoại: donate cho bài nào
        public int PostId { get; set; }
        public Post? Post { get; set; }

        // Khóa ngoại: ai donate
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
