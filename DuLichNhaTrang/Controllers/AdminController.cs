using DuLichNhaTrang.Data;
using DuLichNhaTrang.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DuLichNhaTrang.Controllers
{
    // Chỉ tài khoản có vai trò Admin mới vào được các trang trong Controller này
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: /Admin hoặc /Admin?trangThai=ChoDuyet
        // Hiện danh sách bài để Admin duyệt, từ chối hoặc xóa
        public IActionResult Index(string? trangThai)
        {
            var query = _context.Posts
                .Include(p => p.Category)
                .Include(p => p.User)
                .AsQueryable();

            // Nếu Admin bấm vào một bộ lọc thì chỉ lấy bài có trạng thái đó
            if (!string.IsNullOrEmpty(trangThai))
            {
                query = query.Where(p => p.TrangThai == trangThai);
            }

            // Bài "Chờ duyệt" xếp lên đầu, sau đó mới đến các bài khác (mới nhất trước)
            var danhSach = query
                .OrderBy(p => p.TrangThai == "ChoDuyet" ? 0 : 1)
                .ThenByDescending(p => p.NgayDang)
                .ToList();

            // Số lượng bài của từng trạng thái để hiện trên các nút lọc
            ViewBag.TrangThai = trangThai;
            ViewBag.SoBaiTatCa = _context.Posts.Count();
            ViewBag.SoBaiChoDuyet = _context.Posts.Count(p => p.TrangThai == "ChoDuyet");
            ViewBag.SoBaiDaDuyet = _context.Posts.Count(p => p.TrangThai == "DaDuyet");
            ViewBag.SoBaiTuChoi = _context.Posts.Count(p => p.TrangThai == "TuChoi");

            return View(danhSach);
        }

        // GET: /Admin/ChiTiet/5
        // Admin xem đầy đủ nội dung bài (kể cả bài chưa duyệt) trước khi quyết định
        public IActionResult ChiTiet(int id)
        {
            var post = _context.Posts
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: /Admin/Duyet/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Duyet(int id, string? boLoc)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            post.TrangThai = "DaDuyet";
            _context.SaveChanges();

            TempData["ThongBao"] = "Đã duyệt bài \"" + post.TieuDe + "\". Bài viết đã hiện trên trang chủ.";
            return RedirectToAction("Index", new { trangThai = boLoc });
        }

        // POST: /Admin/TuChoi/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TuChoi(int id, string? boLoc)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            post.TrangThai = "TuChoi";
            _context.SaveChanges();

            TempData["ThongBao"] = "Đã từ chối bài \"" + post.TieuDe + "\".";
            return RedirectToAction("Index", new { trangThai = boLoc });
        }

        // POST: /Admin/Xoa/5
        // Xóa bài vi phạm. Donate của bài cũng bị xóa theo (cấu hình Cascade)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Xoa(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            // Xóa file ảnh do thành viên tải lên (không xóa ảnh mẫu dùng chung)
            XoaAnhDaTaiLen(post.HinhAnh);

            _context.Posts.Remove(post);
            _context.SaveChanges();

            TempData["ThongBao"] = "Đã xóa bài \"" + post.TieuDe + "\".";
            return RedirectToAction("Index");
        }

        // Chỉ xóa file trong wwwroot/images/posts (ảnh do người dùng tải lên)
        private void XoaAnhDaTaiLen(string? hinhAnh)
        {
            if (string.IsNullOrWhiteSpace(hinhAnh))
            {
                return;
            }

            // Ảnh mẫu nằm ở images/sample, dùng chung cho nhiều bài nên không được xóa
            if (!hinhAnh.StartsWith("images/posts/"))
            {
                return;
            }

            var duongDan = Path.Combine(_environment.WebRootPath, hinhAnh.Replace('/', Path.DirectorySeparatorChar));
            if (System.IO.File.Exists(duongDan))
            {
                System.IO.File.Delete(duongDan);
            }
        }
    }
}
