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

        // Trang báo lỗi khi đăng nhập rồi nhưng không đủ quyền
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
