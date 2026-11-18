using DuLichNhaTrang.Data;
using DuLichNhaTrang.Models;
using DuLichNhaTrang.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DuLichNhaTrang.Controllers
{
    // Chỉ thành viên đã đăng nhập mới được donate
    [Authorize]
    public class DonationController : Controller
    {
        private readonly AppDbContext _context;

        // Định dạng số tiền kiểu Việt Nam: 50.000 (dùng cho thông báo)
        private static readonly System.Globalization.CultureInfo VanHoaViet = new System.Globalization.CultureInfo("vi-VN");

        // Mỗi lần donate thì bài được ưu tiên lên đầu trang trong 7 ngày
        private const int SoNgayUuTien = 7;

        public DonationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Donation - trang chọn bài muốn donate
        [HttpGet]
        public IActionResult Index()
        {
            var userId = LayUserId();

            // Chỉ donate cho bài đã duyệt, và nên ưu tiên bài của chính mình
            var danhSachBai = _context.Posts
                .Include(p => p.Category)
                .Where(p => p.TrangThai == "DaDuyet")
                .OrderByDescending(p => p.UserId == userId) // bài của mình lên trước
                .ThenBy(p => p.TieuDe)
                .ToList();

            return View(new DonateViewModel { DanhSachBai = danhSachBai });
        }

        // POST: /Donation - nhận bài đã chọn + số tiền, sang bước xác nhận
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(DonateViewModel model)
        {
            var userId = LayUserId();

            // Kiểm tra bài có tồn tại và đã được duyệt chưa
            var post = _context.Posts
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefault(p => p.Id == model.PostId && p.TrangThai == "DaDuyet");

            if (post == null)
            {
                ModelState.AddModelError("PostId", "Bài viết không tồn tại hoặc chưa được duyệt");
            }

            if (!ModelState.IsValid || post == null)
            {
                // Có lỗi thì hiện lại trang chọn bài kèm thông báo
                model.DanhSachBai = _context.Posts
                    .Include(p => p.Category)
                    .Where(p => p.TrangThai == "DaDuyet")
                    .OrderByDescending(p => p.UserId == userId)
                    .ThenBy(p => p.TieuDe)
                    .ToList();

                return View(model);
            }

            // Không tự donate cho bài của chính mình (tránh tự đẩy bài mình lên đầu)
            if (post.UserId == userId)
            {
                TempData["Loi"] = "Bạn không thể donate cho bài do chính mình đăng. Hãy chọn bài của người khác.";
                return RedirectToAction("Index");
            }

            // Sang trang xác nhận, chưa lưu gì vào CSDL
            return View("XacNhan", new DonateViewModel { PostId = post.Id, SoTien = model.SoTien });
        }

        // GET: /Donation/XacNhan/5?soTien=50000 - xem lại thông tin trước khi xác nhận
        [HttpGet]
        public IActionResult XacNhan(int id, decimal? soTien)
        {
            var post = _context.Posts
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefault(p => p.Id == id && p.TrangThai == "DaDuyet");

            if (post == null)
            {
                return NotFound();
            }

            if (post.UserId == LayUserId())
            {
                TempData["Loi"] = "Bạn không thể donate cho bài do chính mình đăng.";
                return RedirectToAction("Index");
            }

            return View(new DonateViewModel { PostId = post.Id, SoTien = soTien });
        }

        // POST: /Donation/XacNhan - lưu lượt donate và đặt hạn ưu tiên 7 ngày
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XacNhan(DonateViewModel model)
        {
            var userId = LayUserId();
            // Nạp kèm Category/User để hiện lại trang xác nhận nếu dữ liệu sai
            var post = _context.Posts
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefault(p => p.Id == model.PostId && p.TrangThai == "DaDuyet");

            if (post == null)
            {
                return NotFound();
            }

            // Vẫn kiểm tra lại ở server, không tin vào dữ liệu gửi lên
            if (post.UserId == userId)
            {
                TempData["Loi"] = "Bạn không thể donate cho bài do chính mình đăng.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var soTien = model.SoTien ?? 0;

            // Bước 1: lưu lượt donate vào bảng Donations
            _context.Donations.Add(new Donation
            {
                PostId = post.Id,
                UserId = userId,
                SoTien = soTien,
                NgayDonate = DateTime.Now
            });

            // Bước 2: đặt FeaturedUntil = 7 ngày tính từ hôm nay.
            // Nếu bài còn hạn ưu tiên dài hơn (vừa donate trước đó) thì cộng tiếp từ hạn cũ.
            var bayGio = DateTime.Now;
            var mocBatDau = (post.FeaturedUntil != null && post.FeaturedUntil > bayGio)
                ? post.FeaturedUntil.Value
                : bayGio;

            post.FeaturedUntil = mocBatDau.AddDays(SoNgayUuTien);

            _context.SaveChanges();

            TempData["ThongBao"] = "Cảm ơn bạn đã donate " + soTien.ToString("N0", VanHoaViet)
                + " đồng cho bài \"" + post.TieuDe + "\". Bài viết được ưu tiên đến ngày "
                + post.FeaturedUntil.Value.ToString("dd/MM/yyyy") + ".";

            return RedirectToAction("Details", "Home", new { id = post.Id });
        }

        // Lấy Id của người đang đăng nhập (đã lưu trong cookie lúc login)
        private int LayUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }
    }
}
