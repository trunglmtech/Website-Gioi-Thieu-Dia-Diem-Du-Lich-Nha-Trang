using System.Net;
using System.Text.RegularExpressions;

namespace DuLichNhaTrang.Models
{
    // Hàm phụ xử lý mô tả có định dạng HTML (in đậm, in nghiêng, danh sách...)
    public static class NoiDungHtml
    {
        // Bỏ hết thẻ HTML, chỉ giữ chữ thuần
        // Dùng để hiện đoạn tóm tắt ngắn trên thẻ bài viết ở trang chủ
        public static string LayChuThuan(string? html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return "";
            }

            var chu = Regex.Replace(html, "<.*?>", " ", RegexOptions.Singleline);
            chu = WebUtility.HtmlDecode(chu);
            chu = Regex.Replace(chu, "\\s+", " ");

            return chu.Trim();
        }
    }
}
