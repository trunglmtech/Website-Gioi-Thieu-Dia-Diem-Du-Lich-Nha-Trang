# Thư mục cơ sở dữ liệu

| File | Nội dung |
| --- | --- |
| `DuLichNhaTrang.mdf` | File dữ liệu chính của CSDL |
| `DuLichNhaTrang_log.ldf` | File log của SQL Server |
| `DuLichNhaTrang.bak` | File sao lưu (chỉ có khi tự backup) |

Hai file `.mdf` / `.ldf` **tự sinh** khi chạy website lần đầu: Entity Framework tạo CSDL theo chuỗi kết nối
trong `DuLichNhaTrang/appsettings.json`, kèm dữ liệu mẫu (tài khoản, loại địa điểm, bài viết).

CSDL được **gắn (attach)** vào **SQL Server Express** trên máy (`.\SQLEXPRESS`), nhờ vậy mở được bằng
**SQL Server Management Studio (SSMS)** để xem và sửa dữ liệu.

> File `.mdf`, `.ldf`, `.bak` **không đưa lên GitHub** vì là dữ liệu của từng máy (đã khai báo trong `.gitignore`).

## 1. Mở CSDL bằng SSMS

1. Mở **SQL Server Management Studio**.
2. Hộp **Connect to Server**:
   - **Server type**: Database Engine
   - **Server name**: `.\SQLEXPRESS` (nếu không được thì thử `localhost\SQLEXPRESS`)
   - **Authentication**: Windows Authentication
3. Bấm **Connect** → mở **Databases** → chọn `DuLichNhaTrang`.
4. Mở **Tables**, chuột phải bảng cần xem:
   - **Select Top 1000 Rows**: xem dữ liệu
   - **Edit Top 200 Rows**: sửa trực tiếp

Sửa xong nhấn **Ctrl + S** để lưu. Các bảng cần biết:

| Bảng | Nội dung |
| --- | --- |
| `dbo.Users` | Tài khoản: `TenDangNhap`, `MatKhau` (đã hash), `VaiTro` |
| `dbo.Categories` | Loại địa điểm: `TenLoai` |
| `dbo.Posts` | Bài viết: `TieuDe`, `MoTa`, `TrangThai`, `LyDoTuChoi`, `FeaturedUntil` |
| `dbo.Donations` | Lượt donate mô phỏng: `SoTien`, `NgayDonate` |

> ⚠️ `Users.MatKhau` là **mật khẩu đã hash**, không phải chữ thường. Không gõ mật khẩu mới vào cột này.
> Muốn có tài khoản mới, dùng trang **Đăng ký** trên website.

## 2. Câu lệnh SQL hay dùng

Chọn CSDL `DuLichNhaTrang` → **New Query** → dán câu lệnh → **Execute (F5)**.

```sql
-- Xem toàn bộ bài viết
SELECT Id, TieuDe, TrangThai, LyDoTuChoi, NgayDang FROM dbo.Posts ORDER BY Id;

-- Duyệt tất cả bài đang chờ
UPDATE dbo.Posts SET TrangThai = N'DaDuyet', LyDoTuChoi = NULL WHERE TrangThai = N'ChoDuyet';

-- Đẩy một bài lên đầu trang 7 ngày (donate mô phỏng)
UPDATE dbo.Posts SET FeaturedUntil = DATEADD(DAY, 7, GETDATE()) WHERE Id = 6;
```

> Chỉ nên sửa trực tiếp trong SSMS khi demo hoặc sửa dữ liệu mẫu. Khi website đang chạy, nên thao tác
> qua giao diện để đúng quy tắc nghiệp vụ.

## 3. Sao lưu (backup) bằng SSMS

1. Chuột phải CSDL `DuLichNhaTrang` → **Tasks** → **Back Up...**
2. Ở mục **Destination**, bấm **Remove** để bỏ đường dẫn mặc định.
3. Bấm **Add...** → chọn `D:\...\database\DuLichNhaTrang.bak` → **OK**.
4. Bấm **OK** để sao lưu, chờ thông báo *"The backup of database ... completed successfully"*.

> Nên backup trước khi sửa dữ liệu quan trọng hoặc trước buổi demo.

## 4. Khôi phục (restore) bằng SSMS

1. Chuột phải **Databases** → **Restore Database...**
2. Chọn **Device** → bấm **...** → **Add** → chọn file `DuLichNhaTrang.bak` → **OK**.
3. Bấm **OK** để khôi phục.

> Nếu báo lỗi *"database is in use"*, tắt website (dừng app trong Visual Studio) rồi thử lại.

## 5. Máy mới (chưa có file CSDL)

Chỉ cần chạy website một lần: EF tự tạo file CSDL mới và thêm dữ liệu mẫu.

> Nếu di chuyển thư mục dự án sang vị trí khác, cần sửa đường dẫn `AttachDbFilename` trong
> `DuLichNhaTrang/appsettings.json` cho đúng vị trí mới.
>
> Nếu CSDL đang được gắn ở máy khác, cần **detach** ở máy đó (hoặc copy file rồi attach lại) trước khi dùng.
