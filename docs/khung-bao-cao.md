# Khung báo cáo đồ án (dựa trên báo cáo tham khảo)

Báo cáo tham khảo `[studocu.com] - Báo cáo Đồ án Cơ sở 1 - Xây dựng...pdf` (38 trang, đề tài
*Giới thiệu Di sản văn hóa Việt Nam*, trường ĐH CNTT & TT Việt – Hàn) chỉ dựng **giao diện Front-End**,
chưa có cơ sở dữ liệu. Đồ án của mình **đi xa hơn** vì có CSDL, phân quyền và kiểm duyệt.
Vì vậy chỉ **học khung – cách trình bày**, không chép nội dung.

## 1. Cấu trúc báo cáo nên theo

| Phần | Nội dung | Ghi chú |
| --- | --- | --- |
| Bìa | Trường, khoa, tên đồ án, GVHD, sinh viên, lớp, khóa, năm | Đổi tên trường thành ĐH Nha Trang |
| Bìa lót | Giống trang bìa | |
| Nhận xét của GVHD | Khung ký nhận xét | |
| Lời cảm ơn | Cảm ơn thầy cô, nhà trường | Viết ngắn 1 trang |
| Mục lục | Đánh số trang đến cấp 3 | Tạo tự động bằng Word (References → Table of Contents) |
| Danh mục hình ảnh | Hình 1.1, 2.1, 3.1... | Word: Insert Table of Figures |
| Danh mục bảng | Bảng 2.1, 3.1... | Dùng cho bảng dữ liệu, bảng test case |
| **Mở đầu** | Lý do chọn đề tài, mục đích, đối tượng, phạm vi, ý nghĩa | Xem mục 2 dưới |
| **Chương I. Cơ sở lý thuyết** | ASP.NET Core MVC, C#, Entity Framework Core, SQL Server, Bootstrap, Razor | Có thể lấy từ README của mình |
| **Chương II. Phân tích và thiết kế hệ thống** | Yêu cầu chức năng, tác nhân, Use Case, ERD, mô tả bảng, quy tắc nghiệp vụ | Điểm mạnh của mình – **báo cáo tham khảo thiếu hẳn phần này** |
| **Chương III. Xây dựng và triển khai** | Môi trường, kiến trúc MVC, phân quyền, chức năng + ảnh chụp, kiểm thử | Chép ảnh chụp vào `docs/screenshots/` |
| **Kết luận và hướng phát triển** | Đã làm được, hạn chế, hướng phát triển | Bám đúng những gì mình làm |
| Tài liệu tham khảo | Giáo trình, trang Microsoft Learn, W3Schools | |

## 2. Mở đầu – dàn ý viết cho đề tài Nha Trang

- **Lý do chọn đề tài:** Nha Trang là trung tâm du lịch, nhưng thông tin địa điểm còn rải rác, chưa có
  nơi để người dân/du khách cùng đăng bài giới thiệu và có kiểm duyệt. Website giúp tập trung thông tin,
  có vai trò kiểm duyệt để nội dung đáng tin, đồng thời cho phép trả tiền (mô phỏng) để đẩy tin lên đầu.
- **Mục đích:** xây dựng website giới thiệu điểm du lịch; rèn kỹ năng ASP.NET MVC, EF Core, SQL Server.
- **Đối tượng nghiên cứu:** ASP.NET Core MVC, Entity Framework Core, SQL Server, Bootstrap, phân quyền,
  mô hình Client–Server.
- **Phạm vi:** 3 nhóm chức năng theo đề tài; donate chỉ **mô phỏng**; quên mật khẩu hiện chưa gửi email.
- **Ý nghĩa:** giúp quảng bá du lịch địa phương; là môi trường thực hành phát triển web có CSDL.

## 3. Nội dung mình đã có sẵn để đưa vào báo cáo

| Phần báo cáo | Lấy từ file nào trong repo |
| --- | --- |
| Yêu cầu, chức năng, quy tắc nghiệp vụ | `README.md` mục 2, 3 |
| Bảng dữ liệu, quan hệ | `README.md` mục 5 + hình `docs/erd.png` |
| Sơ đồ Use Case | `docs/use-case.png` |
| Kiến trúc MVC, phân quyền | `README.md` mục 6 (luồng MVC) + `Program.cs` |
| Kết quả kiểm thử | `docs/test-case.md` (50 test case) |
| Chức năng từng vai trò | `docs/huong-dan-su-dung.md` |
| Cơ sở dữ liệu, SSMS | `database/README.md` |

## 4. Việc cần làm thêm (chưa có trong repo)

1. **Ảnh chụp màn hình** (báo cáo tham khảo dành hơn 20 trang cho ảnh): trang chủ, chi tiết bài, đăng bài,
   bài của tôi, donate, quản trị, duyệt/từ chối, loại địa điểm, đăng nhập, đăng ký, quên mật khẩu.
   Lưu vào `docs/screenshots/` với tên `hinh-3-1-trang-chu.png`...
2. **Cập nhật hình ERD:** thêm cột `Posts.LyDoTuChoi` và bảng `PasswordResets`.
3. **Viết nội dung chương** (không chép mẫu, mỗi chương 5–10 trang).
4. **Slide demo** (báo cáo tham khảo không có) – cần cho buổi bảo vệ.

## 5. Điểm nên tránh khi viết

- Nhiều lỗi chính tả trong bản tham khảo (ví dụ "thưc hiện", "dến", "phần gìn dữ"). Đọc lại báo cáo của mình.
- Bản tham khảo ghi **Bootstrap 4, React.js** nhưng code của mình là **Bootstrap 5, Razor, C#** – phải viết đúng.
- Không ghi "chỉ làm Front-End" – mình đã có CSDL, đó là điểm cộng.
- Đừng để hình và nội dung lệch (ví dụ Hình 1.4 dùng 2 lần cho CSS và Bootstrap trong bản mẫu).
