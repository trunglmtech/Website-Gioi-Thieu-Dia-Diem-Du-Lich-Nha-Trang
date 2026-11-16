# Thư mục cơ sở dữ liệu

| File | Nội dung |
| --- | --- |
| `DuLichNhaTrang.mdf` | File dữ liệu chính của CSDL |
| `DuLichNhaTrang_log.ldf` | File log của SQL Server |

Hai file này **tự sinh** khi chạy website lần đầu: Entity Framework tạo CSDL theo chuỗi kết nối trong
`DuLichNhaTrang/appsettings.json`, kèm dữ liệu mẫu (tài khoản, loại địa điểm, bài viết).

> File `.mdf` và `.ldf` **không đưa lên GitHub** vì là dữ liệu của từng máy (đã khai báo trong `.gitignore`).

## Sao lưu

1. Tắt website (đóng Visual Studio hoặc dừng app) để SQL Server nhả file.
2. Copy cả thư mục `database/` ra nơi khác (USB, ổ D:, Google Drive...).

## Khôi phục

1. Tắt website.
2. Copy `DuLichNhaTrang.mdf` và `DuLichNhaTrang_log.ldf` vào lại thư mục này.
3. Chạy website.

## Máy mới (chưa có file CSDL)

Chỉ cần chạy website một lần: EF tự tạo file CSDL mới và thêm dữ liệu mẫu.

> Nếu di chuyển thư mục dự án sang vị trí khác, cần sửa đường dẫn `AttachDbFilename` trong
> `DuLichNhaTrang/appsettings.json` cho đúng vị trí mới.
