using DuLichNhaTrang.Data;
using DuLichNhaTrang.Models;
using DuLichNhaTrang.Models.ViewModels;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DuLichNhaTrang.Controllers
{
    // Chỉ người đã đăng nhập mới được đăng bài và xem bài của mình
    [Authorize]
    public class PostController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        // Các đuôi file ảnh được phép tải lên
        private static readonly string[] DuoiAnhChoPhep = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        // Bộ lọc HTML: giữ các thẻ định dạng hợp lệ, xóa <script>, onerror... để chống XSS
        // font-size: cho phép giữ cỡ chữ (12px, 14px...) do người dùng chọn trong khung soạn thảo
        private static readonly HtmlSanitizer BoLocHtml = TaoBoLocHtml();

        private static HtmlSanitizer TaoBoLocHtml()
        {
            var boLoc = new HtmlSanitizer();
            boLoc.AllowedCssProperties.Add("font-size");
            return boLoc;
        }

        public PostController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: /Post/Create - hiển thị form đăng bài
        [HttpGet]
        public IActionResult Create()
        {
            NapDanhSachLoai();
            return View();
        }

        // POST: /Post/Create - nhận dữ liệu form, lưu ảnh và lưu bài vào CSDL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCreateViewModel model)
        {
            // Kiểm tra loại địa điểm có thật trong CSDL không (bỏ qua khi người dùng chưa chọn)
            if (model.CategoryId != 0 && !_context.Categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Loại địa điểm không hợp lệ");
            }

            // Kiểm tra ảnh tải lên: đúng đuôi file và không quá 5 MB
            if (model.HinhAnh != null)
            {
                var duoiFile = Path.GetExtension(model.HinhAnh.FileName).ToLowerInvariant();

                if (!DuoiAnhChoPhep.Contains(duoiFile))
                {
                    ModelState.AddModelError("HinhAnh", "Chỉ nhận file ảnh .jpg, .jpeg, .png, .gif, .webp");
                }
                else if (model.HinhAnh.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("HinhAnh", "Ảnh không được lớn hơn 5 MB");
                }
            }

            // Lọc HTML của mô tả: giữ in đậm, in nghiêng, danh sách... và bỏ mã độc
            model.MoTa = BoLocHtml.Sanitize(model.MoTa ?? "");

            // Khung soạn thảo để trống thường gửi lên "<p><br></p>"
            // nên phải bỏ thẻ HTML rồi kiểm tra xem còn chữ nào không
            if (string.IsNullOrWhiteSpace(NoiDungHtml.LayChuThuan(model.MoTa)))
            {
                ModelState.AddModelError("MoTa", "Vui lòng nhập mô tả");
            }

            if (!ModelState.IsValid)
            {
                // Có lỗi thì hiện lại form kèm thông báo
                NapDanhSachLoai();
                return View(model);
            }

            // Bước 1: tạo thư mục wwwroot/images/posts nếu chưa có
            var thuMucAnh = Path.Combine(_environment.WebRootPath, "images", "posts");
            Directory.CreateDirectory(thuMucAnh);

            // Bước 2: đặt tên file mới để không trùng (Guid) và giữ đuôi file
            var tenFile = Guid.NewGuid().ToString("N") + Path.GetExtension(model.HinhAnh!.FileName).ToLowerInvariant();
            var duongDanDayDu = Path.Combine(thuMucAnh, tenFile);

            // Bước 3: copy file người dùng gửi lên vào thư mục ảnh
            using (var stream = new FileStream(duongDanDayDu, FileMode.Create))
            {
                await model.HinhAnh.CopyToAsync(stream);
            }

            // Bước 4: lấy Id của người đang đăng nhập (đã lưu trong cookie lúc login)
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Bước 5: tạo bài mới, trạng thái luôn là Chờ duyệt
            var post = new Post
            {
                TieuDe = model.TieuDe,
                MoTa = model.MoTa,
                HinhAnh = "images/posts/" + tenFile,
                TrangThai = "ChoDuyet",
                NgayDang = DateTime.Now,
                UserId = userId,
                CategoryId = model.CategoryId
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            TempData["ThongBao"] = "Đăng bài thành công. Bài viết đang chờ Admin duyệt.";
            return RedirectToAction("MyPosts");
        }

        // GET: /Post/MyPosts - chỉ hiện bài của chính người đang đăng nhập
        [HttpGet]
        public IActionResult MyPosts()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var danhSach = _context.Posts
                .Include(p => p.Category)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.NgayDang)
                .ToList();

            return View(danhSach);
        }

        // Đổ danh sách loại địa điểm vào dropdown
        private void NapDanhSachLoai()
        {
            var danhSachLoai = _context.Categories.OrderBy(c => c.TenLoai).ToList();
            ViewBag.Categories = new SelectList(danhSachLoai, "Id", "TenLoai");
        }
    }
}
