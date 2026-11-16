using System.ComponentModel.DataAnnotations;

namespace DuLichNhaTrang.Models
{
    // Bảng Categories: loại địa điểm du lịch (bãi biển, chùa, khu vui chơi...)
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên loại địa điểm")]
        [StringLength(100, ErrorMessage = "Tên loại tối đa 100 ký tự")]
        public string TenLoai { get; set; } = "";

        // Một loại địa điểm có nhiều bài viết
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
