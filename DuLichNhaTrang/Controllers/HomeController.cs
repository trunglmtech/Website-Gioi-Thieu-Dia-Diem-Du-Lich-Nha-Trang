using DuLichNhaTrang.Data;
using DuLichNhaTrang.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DuLichNhaTrang.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: / hoặc /?q=từ khóa
        // Trang chủ: chỉ hiện bài Đã duyệt, bài donate còn hạn lên đầu, còn lại xếp ngẫu nhiên
        public IActionResult Index(string? q = null)
        {
            // Bước 1: lấy các bài đã được Admin duyệt
            var query = _context.Posts
                .Include(p => p.Category)
                .Where(p => p.TrangThai == "DaDuyet");

            // Bước 2: nếu có từ khóa thì lọc theo tiêu đề
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(p => p.TieuDe.Contains(q));
            }

            var danhSach = query.ToList();

            var bayGio = DateTime.Now;

            // Bước 3: bài có donate còn hiệu lực (FeaturedUntil > bây giờ) xếp lên đầu
            var noiBat = danhSach
                .Where(p => p.FeaturedUntil != null && p.FeaturedUntil > bayGio)
                .OrderByDescending(p => p.FeaturedUntil)
                .ToList();

            // Bước 4: các bài còn lại xếp ngẫu nhiên mỗi lần tải trang
            var conLai = danhSach
                .Where(p => p.FeaturedUntil == null || p.FeaturedUntil <= bayGio)
                .OrderBy(p => Guid.NewGuid())
                .ToList();

            ViewBag.TuKhoa = q;
            return View(noiBat.Concat(conLai).ToList());
        }

        // GET: /Home/Details/5 - xem chi tiết một bài đã duyệt
        public IActionResult Details(int id)
        {
            var post = _context.Posts
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefault(p => p.Id == id && p.TrangThai == "DaDuyet");

            // Không tìm thấy hoặc bài chưa duyệt thì trả về lỗi 404
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
