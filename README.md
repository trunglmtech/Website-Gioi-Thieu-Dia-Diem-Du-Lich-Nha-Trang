<div align="center">

# 🌴 Du Lịch Nha Trang

**Website giới thiệu du lịch Nha Trang xây dựng bằng ASP.NET MVC**

Chia sẻ điểm đến, ẩm thực và trải nghiệm tại thành phố biển Nha Trang

![ASP.NET MVC](https://img.shields.io/badge/ASP.NET-MVC-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?logo=bootstrap&logoColor=white)
![Status](https://img.shields.io/badge/Tr%E1%BA%A1ng%20th%C3%A1i-%C4%90ang%20ph%C3%A1t%20tri%E1%BB%83n-yellow)

</div>

---

## 📖 Giới thiệu

**Du Lịch Nha Trang** là đồ án cơ sở ngành của sinh viên ngành Công nghệ thông tin, Trường Đại học Nha Trang.

Website cho phép **thành viên đăng bài** giới thiệu các địa điểm du lịch ở Nha Trang. Bài viết được **quản trị viên kiểm duyệt** trước khi hiển thị. Các bài đã duyệt hiển thị theo **thứ tự ngẫu nhiên**, và bài có **donate** sẽ được ưu tiên đưa lên đầu trang.

## ✨ Chức năng

### 👤 Khách
- Xem danh sách bài giới thiệu
- Xem chi tiết từng bài
- Tìm kiếm theo tên
- Đăng ký tài khoản

### 🧑‍💻 Thành viên
- Đăng nhập
- Đăng bài (tiêu đề, mô tả, hình ảnh, loại địa điểm)
- Xem bài của mình và trạng thái (chờ duyệt, đã duyệt, từ chối)
- Donate để đẩy bài lên đầu trang *(mô phỏng, không dùng thanh toán thật)*

### 🛡️ Quản trị viên (Admin)
- Duyệt hoặc từ chối bài viết
- Xóa bài vi phạm
- Quản lý loại địa điểm (thêm, sửa, xóa)

## 🔄 Quy tắc hiển thị bài

1. Chỉ hiển thị bài ở trạng thái **Đã duyệt**.
2. Bài có donate còn hiệu lực (7 ngày) được xếp **ở đầu trang**.
3. Các bài còn lại hiển thị **ngẫu nhiên** mỗi lần tải trang.

## 🛠️ Công nghệ sử dụng

| Thành phần | Công nghệ |
|---|---|
| Nền tảng | ASP.NET MVC (C#) |
| Cơ sở dữ liệu | SQL Server |
| Truy cập dữ liệu | Entity Framework |
| Giao diện | HTML, CSS, Bootstrap |
| Quản lý mã nguồn | Git, GitHub |

## 🗂️ Thiết kế

### Sơ đồ Use Case

![Use Case](docs/use_case_du_lich_nha_trang.png)

### Sơ đồ ERD

![ERD](docs/erd_du_lich_nha_trang.png)

### Cơ sở dữ liệu

| Bảng | Mô tả |
|---|---|
| `Users` | Tài khoản người dùng và vai trò (Admin, Member) |
| `Categories` | Loại địa điểm (biển đảo, di tích, ẩm thực...) |
| `Posts` | Bài viết giới thiệu, trạng thái duyệt, thời hạn nổi bật |
| `Donations` | Lịch sử donate đẩy bài lên đầu trang |

## 🚀 Cài đặt và chạy thử

**Yêu cầu:** Visual Studio (có gói *ASP.NET and web development*), SQL Server hoặc SQL Server LocalDB.

```bash
# 1. Tải mã nguồn
git clone <đường-dẫn-repo-của-bạn>

# 2. Mở file .sln bằng Visual Studio
```

3. Mở file cấu hình (`appsettings.json` hoặc `Web.config`) và chỉnh **chuỗi kết nối** (connection string) trỏ đến SQL Server của bạn.
4. Tạo cơ sở dữ liệu bằng migration:
   - Visual Studio: mở *Package Manager Console* và chạy `Update-Database`
   - Hoặc dùng dòng lệnh: `dotnet ef database update`
5. Nhấn **F5** để chạy website.

> Tài khoản Admin mặc định sẽ được cập nhật tại đây sau khi hoàn thành chức năng đăng nhập.

## 📁 Cấu trúc thư mục

```
├── Controllers/     # Xử lý yêu cầu người dùng
├── Models/          # Lớp dữ liệu và truy vấn CSDL
├── Views/           # Giao diện (Razor + Bootstrap)
├── Data/            # DbContext và migrations
├── wwwroot/         # CSS, JS, hình ảnh
├── docs/            # Sơ đồ Use Case, ERD, tài liệu
└── README.md
```

## 🗓️ Tiến độ

- [x] Chọn đề tài, tạo repo và project ASP.NET MVC
- [x] Phân tích yêu cầu, vẽ Use Case và ERD
- [ ] Models, DbContext và cơ sở dữ liệu
- [ ] Đăng ký, đăng nhập, phân quyền
- [ ] Xem danh sách, chi tiết, tìm kiếm
- [ ] Đăng bài và quản lý bài của tôi
- [ ] Admin duyệt bài, quản lý loại địa điểm
- [ ] Hiển thị ngẫu nhiên và donate
- [ ] Kiểm thử và hoàn thiện
- [ ] Báo cáo và demo

## 📸 Hình ảnh giao diện

*Sẽ cập nhật sau khi hoàn thành giao diện.*

<!-- Ví dụ: ![Trang chủ](docs/screenshots/trang-chu.png) -->

## 👨‍🎓 Tác giả

**Trung** – Sinh viên Công nghệ thông tin (chuyên ngành Công nghệ phần mềm), Trường Đại học Nha Trang

Đồ án cơ sở ngành, thực hiện dưới sự hướng dẫn của giảng viên bộ môn.

## 📄 Ghi chú

Đây là sản phẩm phục vụ học tập. Chức năng donate chỉ là **mô phỏng**, không xử lý giao dịch tiền thật.
