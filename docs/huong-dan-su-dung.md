# Hướng dẫn sử dụng

Website **Giới thiệu du lịch Nha Trang** có 3 nhóm người dùng: **Khách**, **Thành viên** và **Quản trị viên (Admin)**.

## 1. Chuẩn bị trước khi dùng

1. Mở solution `DuLichNhaTrang.slnx` bằng Visual Studio 2026, nhấn **F5**.
2. Trình duyệt mở trang chủ (thường là `https://localhost:7131`).
3. Tài khoản mẫu:

| Vai trò | Tên đăng nhập | Mật khẩu |
| --- | --- | --- |
| Quản trị viên | `admin` | `123456` |
| Thành viên | `trung` | `123456` |

> Dữ liệu mẫu gồm 5 loại địa điểm, 6 bài viết (1 bài **Chờ duyệt**), 1 lượt donate.

## 2. Khách (chưa đăng nhập)

| Việc muốn làm | Cách làm |
| --- | --- |
| Xem danh sách địa điểm | Mở trang chủ. Chỉ bài **Đã duyệt** mới hiện; bài donate còn hạn ở đầu, còn lại đổi thứ tự mỗi lần tải trang |
| Tìm địa điểm | Gõ tên vào ô **Tìm địa điểm theo tên...** → **Tìm kiếm** |
| Xem chi tiết | Bấm **Xem chi tiết** trên thẻ bài |
| Đăng ký tài khoản | Bấm **Đăng ký** ở góc phải → nhập tên đăng nhập (3–50 ký tự) và mật khẩu (từ 6 ký tự) → **Đăng ký** |

> Khách **không** thấy nút **Donate**, không vào được trang đăng bài hay trang quản trị.

## 3. Thành viên (đã đăng nhập)

Đăng nhập bằng tài khoản đã đăng ký.

### 3.1 Đăng bài

1. Menu **Đăng bài**.
2. Nhập:
   - **Tiêu đề** (tối đa 200 ký tự)
   - **Loại địa điểm** (Điểm vui chơi, Điểm ăn uống, Điểm tham quan, Điểm nghỉ dưỡng, Điểm cà phê)
   - **Hình ảnh**: `.jpg`, `.jpeg`, `.png`, `.gif`, `.webp`, tối đa 5 MB
   - **Mô tả**: dùng thanh công cụ để in đậm, in nghiêng, chọn cỡ chữ, tạo danh sách...
3. Bấm **Gửi bài chờ duyệt**. Bài được lưu ở trạng thái **Chờ duyệt** và chuyển sang **Bài của tôi**.

### 3.2 Xem bài của tôi

Menu **Bài của tôi** hiện các bài bạn đã đăng kèm trạng thái:

| Trạng thái | Ý nghĩa |
| --- | --- |
| **Chờ duyệt** | Đang đợi Admin xem xét |
| **Đã duyệt** | Bài đã hiện trên trang chủ |
| **Từ chối** | Bài bị trả lại, có dòng **Lý do: ...** để biết cần sửa gì |

### 3.3 Donate để đẩy bài lên đầu trang *(mô phỏng)*

**Cách 1 – donate nhanh 50.000đ:** bấm nút vàng **Donate** trên thẻ bài ở trang chủ, hoặc ở trang chi tiết, hoặc trong **Bài của tôi**.

**Cách 2 – tự chọn số tiền:**

1. Menu **Donate**.
2. Chọn bài muốn donate (bài của chính mình bị chặn).
3. Bấm nút số tiền gợi ý (20.000 / 50.000 / 100.000 / 200.000) hoặc gõ số tiền (1.000 – 100.000.000).
4. Bấm **Tiếp tục** → kiểm tra thông tin → **Xác nhận donate**.
5. Bài được ưu tiên lên đầu trang chủ trong **7 ngày** (donate lần nữa thì cộng tiếp 7 ngày).

> Đây là giao dịch **mô phỏng**, không trừ tiền thật.

### 3.4 Quên mật khẩu

1. Trang **Đăng nhập** → bấm **Quên mật khẩu?**
2. Nhập tên đăng nhập → **Nhận mã đặt lại**.
3. Màn hình hiện **mã 6 số** (mã dùng 15 phút, dùng 1 lần).
4. Nhập mã + mật khẩu mới (2 lần) → **Đặt lại mật khẩu**.
5. Đăng nhập lại bằng mật khẩu mới.

> Hiện tại chưa gửi email nên mã hiện ngay trên màn hình để demo.

## 4. Quản trị viên (Admin)

### 4.1 Duyệt bài

1. Menu **Quản trị** (tab **Bài viết**).
2. Bấm bộ lọc **Chờ duyệt**.
3. Bấm **Xem** để đọc đầy đủ bài.
4. Chọn:
   - **Duyệt bài**: bài hiện trên trang chủ.
   - **Từ chối bài**: **bắt buộc nhập lý do** (tối đa 500 ký tự) → bấm **Từ chối bài**. Lý do hiện cho người đăng trong **Bài của tôi**.
   - **Xóa bài**: hỏi xác nhận, xóa cả ảnh đã tải lên.

### 4.2 Quản lý loại địa điểm

Vào **Quản trị** → tab **Loại địa điểm**.

| Việc | Cách làm |
| --- | --- |
| Thêm | Nhập tên loại mới → **Thêm loại**. Tên không được trùng, tối đa 100 ký tự |
| Sửa | Bấm **Sửa** → đổi tên → **Lưu thay đổi** |
| Xóa | Bấm **Xóa**. Loại còn bài viết thì nút bị khóa và hệ thống báo không xóa được |

> Loại tổng số bài viết được hiển thị ở cột **Số bài viết**.

## 5. Xem dữ liệu trong SSMS

- Kết nối: **Server name** `.\SQLEXPRESS`, **Windows Authentication**.
- CSDL `DuLichNhaTrang` → **Tables** → chuột phải bảng → **Select Top 1000 Rows**.

| Bảng | Xem gì |
| --- | --- |
| `Users` | Tài khoản, vai trò (mật khẩu là hash, không đọc được) |
| `Categories` | Loại địa điểm |
| `Posts` | Bài viết, trạng thái, lý do từ chối, hạn ưu tiên |
| `Donations` | Lịch sử donate mô phỏng |
| `PasswordResets` | Mã đặt lại mật khẩu |

> Chi tiết phân quyền / xóa tài khoản: xem `database/README.md`.

## 6. Bảng kiểm thử

Kết quả kiểm thử các chức năng (đăng ký, đăng nhập, phân quyền, đăng bài, duyệt bài, tìm kiếm, donate, quên mật khẩu...) ghi trong `docs/test-case.md`.

## 7. Xử lý lỗi thường gặp

| Lỗi | Nguyên nhân | Cách khắc phục |
| --- | --- | --- |
| Không mở được trang | Chưa chạy website hoặc bấm F5 khi đang build | Chạy lại bằng Visual Studio |
| Báo lỗi kết nối CSDL | SQL Server Express chưa chạy, hoặc đường dẫn `AttachDbFilename` sai | Mở **Services**, khởi động `SQL Server (SQLEXPRESS)`; kiểm tra `appsettings.json` |
| Đăng nhập đúng mật khẩu vẫn không vào | `MatKhau` trong CSDL bị sửa tay thành chữ thường | Xóa tài khoản rồi đăng ký lại (xem `database/README.md`) |
| Ảnh không hiện | File không nằm trong `wwwroot/images/posts` | Đăng bài lại, chỉ nhận file ảnh |
| Phải đổi vai trò mà chưa có tác dụng | Cookie đăng nhập cũ vẫn giữ vai trò cũ | Đăng xuất rồi đăng nhập lại |
