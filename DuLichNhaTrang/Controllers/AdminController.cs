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
            // Duyệt lại thì xóa lý do từ chối cũ (nếu trước đó bài đã bị từ chối)
            post.LyDoTuChoi = null;
            _context.SaveChanges();

            TempData["ThongBao"] = "Đã duyệt bài \"" + post.TieuDe + "\". Bài viết đã hiện trên trang chủ.";
            return RedirectToAction("Index", new { trangThai = boLoc });
        }

        // POST: /Admin/TuChoi/5
        // Từ chối bài và lưu lý do để thành viên biết cần sửa gì
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TuChoi(int id, string? lyDo)
        {
            // Phải nạp kèm Category/User để hiện lại trang chi tiết nếu thiếu lý do
            var post = _context.Posts
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            lyDo = lyDo?.Trim();

            if (string.IsNullOrEmpty(lyDo))
            {
                ViewBag.LyDoNhap = lyDo;
                ModelState.AddModelError("lyDo", "Vui lòng nhập lý do từ chối để thành viên biết cần sửa gì");
                return View("ChiTiet", post);
            }

            if (lyDo.Length > 500)
            {
                ViewBag.LyDoNhap = lyDo;
                ModelState.AddModelError("lyDo", "Lý do từ chối tối đa 500 ký tự");
                return View("ChiTiet", post);
            }

            post.TrangThai = "TuChoi";
            post.LyDoTuChoi = lyDo;
            _context.SaveChanges();

            TempData["ThongBao"] = "Đã từ chối bài \"" + post.TieuDe + "\".";
            return RedirectToAction("Index");
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

        // GET: /Admin/LoaiDiaDiem
        // Danh sách loại địa điểm kèm số bài viết của từng loại
        public IActionResult LoaiDiaDiem()
        {
            return View(LayDanhSachLoai());
        }

        // POST: /Admin/ThemLoai
        // Thêm một loại địa điểm mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemLoai(Category model)
        {
            // Bỏ khoảng trắng ở hai đầu tên loại trước khi kiểm tra
            model.TenLoai = (model.TenLoai ?? "").Trim();

            // Không cho trùng tên loại đã có
            if (!string.IsNullOrEmpty(model.TenLoai)
                && _context.Categories.Any(c => c.TenLoai == model.TenLoai))
            {
                ModelState.AddModelError("TenLoai", "Loại địa điểm này đã có, vui lòng nhập tên khác");
            }

            if (!ModelState.IsValid)
            {
                // Nhập sai thì hiện lại danh sách kèm thông báo lỗi và giữ chữ vừa nhập
                ViewBag.TenLoaiNhap = model.TenLoai;
                return View("LoaiDiaDiem", LayDanhSachLoai());
            }

            _context.Categories.Add(model);
            _context.SaveChanges();

            TempData["ThongBao"] = "Đã thêm loại địa điểm \"" + model.TenLoai + "\".";
            return RedirectToAction("LoaiDiaDiem");
        }

        // GET: /Admin/SuaLoai/5
        // Mở form sửa tên loại địa điểm
        public IActionResult SuaLoai(int id)
        {
            var loai = _context.Categories.Find(id);
            if (loai == null)
            {
                return NotFound();
            }

            return View(loai);
        }

        // POST: /Admin/SuaLoai
        // Lưu tên loại địa điểm sau khi sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaLoai(Category model)
        {
            var loai = _context.Categories.Find(model.Id);
            if (loai == null)
            {
                return NotFound();
            }

            model.TenLoai = (model.TenLoai ?? "").Trim();

            // Không cho trùng tên với loại KHÁC (bỏ qua chính nó)
            if (!string.IsNullOrEmpty(model.TenLoai)
                && _context.Categories.Any(c => c.Id != model.Id && c.TenLoai == model.TenLoai))
            {
                ModelState.AddModelError("TenLoai", "Loại địa điểm này đã có, vui lòng nhập tên khác");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            loai.TenLoai = model.TenLoai;
            _context.SaveChanges();

            TempData["ThongBao"] = "Đã sửa loại địa điểm thành \"" + loai.TenLoai + "\".";
            return RedirectToAction("LoaiDiaDiem");
        }

        // POST: /Admin/XoaLoai/5
        // Xóa loại địa điểm. Chỉ xóa được khi loại đó KHÔNG còn bài viết nào
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaLoai(int id)
        {
            // Nạp kèm Posts để biết loại này còn bài viết hay không
            var loai = _context.Categories
                .Include(c => c.Posts)
                .FirstOrDefault(c => c.Id == id);

            if (loai == null)
            {
                return NotFound();
            }

            // Còn bài viết thì không cho xóa để tránh mất dữ liệu
            if (loai.Posts.Count > 0)
            {
                TempData["Loi"] = "Không xóa được \"" + loai.TenLoai + "\" vì đang có "
                    + loai.Posts.Count + " bài viết. Hãy xóa hoặc chuyển các bài đó sang loại khác trước.";
                return RedirectToAction("LoaiDiaDiem");
            }

            _context.Categories.Remove(loai);
            _context.SaveChanges();

            TempData["ThongBao"] = "Đã xóa loại địa điểm \"" + loai.TenLoai + "\".";
            return RedirectToAction("LoaiDiaDiem");
        }

        // Lấy danh sách loại địa điểm kèm số bài viết (dùng cho trang quản lý loại)
        private List<Category> LayDanhSachLoai()
        {
            return _context.Categories
                .Include(c => c.Posts)
                .OrderBy(c => c.TenLoai)
                .ToList();
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
