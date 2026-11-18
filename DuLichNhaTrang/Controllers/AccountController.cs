using DuLichNhaTrang.Data;
using DuLichNhaTrang.Models;
using DuLichNhaTrang.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DuLichNhaTrang.Controllers
{
    // Controller xử lý đăng ký, đăng nhập, đăng xuất
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register - hiển thị form đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register - nhận dữ liệu form đăng ký
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra tên đăng nhập đã có người dùng chưa
            bool daTonTai = _context.Users.Any(u => u.TenDangNhap == model.TenDangNhap);
            if (daTonTai)
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập này đã được sử dụng");
                return View(model);
            }

            // Tạo tài khoản mới, mật khẩu được HASH trước khi lưu
            var user = new User
            {
                TenDangNhap = model.TenDangNhap,
                VaiTro = "Member"   // Người tự đăng ký luôn là Thành viên
            };

            var hasher = new PasswordHasher<User>();
            user.MatKhau = hasher.HashPassword(user, model.MatKhau);

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["ThongBao"] = "Đăng ký thành công, mời bạn đăng nhập";
            return RedirectToAction("Login");
        }

        // GET: /Account/Login - hiển thị form đăng nhập
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // Trang người dùng muốn vào trước khi bị chuyển về đây
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login - kiểm tra tài khoản, mật khẩu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Users.FirstOrDefault(u => u.TenDangNhap == model.TenDangNhap);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng");
                return View(model);
            }

            // So sánh mật khẩu người dùng nhập với mật khẩu đã hash trong CSDL
            var hasher = new PasswordHasher<User>();
            var ketQua = hasher.VerifyHashedPassword(user, user.MatKhau, model.MatKhau);
            if (ketQua == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng");
                return View(model);
            }

            // Tạo "thẻ nhận diện" (claims) để biết đang đăng nhập là ai, vai trò gì
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim(ClaimTypes.Role, user.VaiTro)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = model.GhiNho });

            // Nếu trước đó bị chuyển hướng từ trang khác thì quay lại trang đó
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout - đăng xuất
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/QuenMatKhau - bước 1: nhập tên đăng nhập
        [HttpGet]
        public IActionResult QuenMatKhau()
        {
            return View();
        }

        // POST: /Account/QuenMatKhau - tạo mã đặt lại 6 số
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult QuenMatKhau(QuenMatKhauViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Users.FirstOrDefault(u => u.TenDangNhap == model.TenDangNhap);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy tài khoản này");
                return View(model);
            }

            // Mỗi tài khoản chỉ cần 1 mã còn hiệu lực -> xóa các mã cũ
            var maCu = _context.PasswordResets.Where(r => r.UserId == user.Id).ToList();
            _context.PasswordResets.RemoveRange(maCu);

            // Tạo mã 6 số ngẫu nhiên, hết hạn sau 15 phút
            var ma = new PasswordReset
            {
                UserId = user.Id,
                MaDatLai = TaoMa6So(),
                HetHan = DateTime.Now.AddMinutes(15),
                NgayYeuCau = DateTime.Now,
                DaDung = false
            };

            _context.PasswordResets.Add(ma);
            _context.SaveChanges();

            // Giai đoạn này CHƯA gửi email nên hiện mã ngay trên màn hình cho mục đích demo.
            // Khi làm phần gửi mail, chỉ cần gửi ma.MaDatLai qua email và KHÔNG hiện mã ở đây nữa.
            TempData["MaDemo"] = ma.MaDatLai;
            return RedirectToAction("NhapMaDatLai", new { tenDangNhap = user.TenDangNhap });
        }

        // GET: /Account/NhapMaDatLai - bước 2: nhập mã + mật khẩu mới
        [HttpGet]
        public IActionResult NhapMaDatLai(string? tenDangNhap)
        {
            // Mã demo chỉ hiện 1 lần sau khi tạo, chuyển từ TempData sang ViewBag
            ViewBag.MaDemo = TempData["MaDemo"];
            return View(new DatLaiMatKhauViewModel { TenDangNhap = tenDangNhap ?? "" });
        }

        // POST: /Account/NhapMaDatLai - kiểm tra mã và lưu mật khẩu mới (đã hash)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NhapMaDatLai(DatLaiMatKhauViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Users.FirstOrDefault(u => u.TenDangNhap == model.TenDangNhap);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy tài khoản này");
                return View(model);
            }

            // Tìm mã đúng của tài khoản, chưa dùng và còn hạn
            var bayGio = DateTime.Now;
            var ma = _context.PasswordResets
                .Where(r => r.UserId == user.Id
                            && r.MaDatLai == model.MaDatLai.Trim()
                            && !r.DaDung
                            && r.HetHan > bayGio)
                .OrderByDescending(r => r.Id)
                .FirstOrDefault();

            if (ma == null)
            {
                ModelState.AddModelError("MaDatLai", "Mã không đúng hoặc đã hết hạn (mã chỉ dùng trong 15 phút)");
                return View(model);
            }

            // Đổi mật khẩu: hash mật khẩu mới rồi lưu, không lưu chữ thường
            var hasher = new PasswordHasher<User>();
            user.MatKhau = hasher.HashPassword(user, model.MatKhauMoi);

            // Đánh dấu mã đã dùng để không tái sử dụng được
            ma.DaDung = true;

            _context.SaveChanges();

            TempData["ThongBao"] = "Đặt lại mật khẩu thành công, mời bạn đăng nhập bằng mật khẩu mới";
            return RedirectToAction("Login");
        }

        // Sinh mã gồm 6 chữ số (000000 - 999999)
        private static string TaoMa6So()
        {
            return Random.Shared.Next(0, 1000000).ToString("D6");
        }


        // Trang báo lỗi khi đăng nhập rồi nhưng không đủ quyền
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
