/* =====================================================================
   Quản lý tài khoản trong SSMS - chạy trên CSDL DuLichNhaTrang
   Mỗi lần chỉ bôi đen MỘT đoạn rồi bấm Execute (F5).
   ===================================================================== */

-- 1) Xem toàn bộ tài khoản
SELECT Id, TenDangNhap, VaiTro,
       CASE WHEN LEN(MatKhau) > 60 THEN 'da hash' ELSE 'CHUA HASH' END AS MatKhau
FROM dbo.Users
ORDER BY Id;

-- 2) Cấp quyền Admin cho một tài khoản (Admin được duyệt bài, quản lý loại địa điểm)
UPDATE dbo.Users SET VaiTro = N'Admin' WHERE TenDangNhap = N'trung';

-- 3) Hạ quyền Admin về Member (Member chỉ được đăng bài và donate)
-- UPDATE dbo.Users SET VaiTro = N'Member' WHERE TenDangNhap = N'trung';

-- 4) Đếm dữ liệu của một tài khoản trước khi xóa
-- SELECT u.TenDangNhap,
--        (SELECT COUNT(*) FROM dbo.Posts p WHERE p.UserId = u.Id)     AS SoBaiViet,
--        (SELECT COUNT(*) FROM dbo.Donations d WHERE d.UserId = u.Id) AS SoLanDonate
-- FROM dbo.Users u
-- WHERE u.TenDangNhap = N'taikhoanthu';

-- 5) Xóa tài khoản: chỉ xóa được khi đã xử lý hết bài viết và donate của tài khoản đó
--    Bước 5a: xóa các lượt donate của tài khoản
-- DELETE FROM dbo.Donations WHERE UserId = (SELECT Id FROM dbo.Users WHERE TenDangNhap = N'taikhoanthu');
--    Bước 5b: xóa các bài viết của tài khoản
-- DELETE FROM dbo.Posts WHERE UserId = (SELECT Id FROM dbo.Users WHERE TenDangNhap = N'taikhoanthu');
--    Bước 5c: xóa tài khoản
-- DELETE FROM dbo.Users WHERE TenDangNhap = N'taikhoanthu';

-- 6) Đếm số tài khoản theo vai trò
SELECT VaiTro, COUNT(*) AS SoTaiKhoan FROM dbo.Users GROUP BY VaiTro;
