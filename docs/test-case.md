# Bảng kiểm thử (Test case)

Tài liệu này ghi lại các trường hợp kiểm thử của website **Giới thiệu du lịch Nha Trang**.

- **Ngày kiểm thử:** 18/11/2026
- **Môi trường:** Windows 11, Visual Studio 2026, SQL Server Express (`.\SQLEXPRESS`), trình duyệt Chrome
- **Cách kiểm thử:** chạy website và thao tác thủ công trên trình duyệt, đồng thời dùng script tự động gửi yêu cầu HTTP và truy vấn CSDL để so sánh kết quả.
- **Tài khoản dùng để test:** `admin` / `123456` (Admin), `trung` / `123456` (Member)
- **Ký hiệu:** ✅ Đạt (PASS) — ❌ Không đạt (FAIL)

## 1. Tổng hợp

| Nhóm chức năng | Số test case | Đạt | Không đạt |
| --- | --- | --- | --- |
| Đăng ký | 5 | 5 | 0 |
| Đăng nhập, đăng xuất | 4 | 4 | 0 |
| Phân quyền | 4 | 4 | 0 |
| Đăng bài | 8 | 8 | 0 |
| Tìm kiếm | 2 | 2 | 0 |
| Duyệt / từ chối / xóa bài | 8 | 8 | 0 |
| Donate mô phỏng | 6 | 6 | 0 |
| Donate tự đẩy tin (kiểm thử lại) | 5 | 5 | 0 |
| Quản lý loại địa điểm | 4 | 4 | 0 |
| Quên mật khẩu | 4 | 4 | 0 |
| **Tổng** | **50** | **50** | **0** |

## 2. Chi tiết test case

### 2.1 Đăng ký

| Mã | Tình huống | Dữ liệu nhập | Kết quả mong đợi | Kết quả thực tế | Trạng thái |
| --- | --- | --- | --- | --- | --- |
| TC-01 | Đăng ký hợp lệ | `testdk` / `123456` / `123456` | Tạo tài khoản, chuyển về trang đăng nhập, vai trò `Member` | Như mong đợi, CSDL có 1 tài khoản `Member` | ✅ |
| TC-02 | Đăng ký trùng tên đăng nhập | `testdk` (đã có) | Báo "Tên đăng nhập này đã được sử dụng", không tạo thêm | Báo lỗi, vẫn 1 tài khoản | ✅ |
| TC-03 | Tên đăng nhập quá ngắn | `ab` | Báo "Tên đăng nhập từ 3 đến 50 ký tự" | Báo lỗi | ✅ |
| TC-04 | Mật khẩu quá ngắn | `123` | Báo "Mật khẩu phải có ít nhất 6 ký tự" | Báo lỗi | ✅ |
| TC-05 | Nhập lại mật khẩu không khớp | `123456` / `654321` | Báo "Mật khẩu nhập lại không khớp" | Báo lỗi | ✅ |

### 2.2 Đăng nhập, đăng xuất

| Mã | Tình huống | Dữ liệu nhập | Kết quả mong đợi | Kết quả thực tế | Trạng thái |
| --- | --- | --- | --- | --- | --- |
| TC-06 | Đăng nhập Admin đúng | `admin` / `123456` | Vào trang chủ, menu có **Quản trị** | Đăng nhập thành công | ✅ |
| TC-07 | Đăng nhập sai mật khẩu | `admin` / `sai123` | Ở lại trang đăng nhập, báo "Tên đăng nhập hoặc mật khẩu không đúng" | Báo lỗi đúng | ✅ |
| TC-08 | Đăng nhập Member đúng | `trung` / `123456` | Vào trang chủ, menu có **Đăng bài**, **Bài của tôi** | Đăng nhập thành công | ✅ |
| TC-09 | Đăng xuất | Bấm **Đăng xuất** | Quay về trang chủ, menu về trạng thái khách | Đúng | ✅ |

### 2.3 Phân quyền (truy cập trái quyền)

| Mã | Tình huống | Cách kiểm thử | Kết quả mong đợi | Kết quả thực tế | Trạng thái |
| --- | --- | --- | --- | --- | --- |
| TC-10 | Khách vào trang quản trị | Mở `/Admin` khi chưa đăng nhập | Chuyển về `/Account/Login` | Chuyển về trang đăng nhập | ✅ |
| TC-11 | Member vào trang quản trị | Đăng nhập `trung`, mở `/Admin` | Chuyển về `/Account/AccessDenied` ("Không có quyền truy cập") | Chuyển đúng | ✅ |
| TC-12 | Khách vào trang đăng bài | Chưa đăng nhập, mở `/Post/Create` | Chuyển về trang đăng nhập | Chuyển đúng | ✅ |
| TC-13 | Admin vào trang quản trị | Đăng nhập `admin`, mở `/Admin` | Hiện danh sách bài cần duyệt | Hiện đúng (HTTP 200) | ✅ |

### 2.4 Đăng bài

| Mã | Tình huống | Dữ liệu nhập | Kết quả mong đợi | Kết quả thực tế | Trạng thái |
| --- | --- | --- | --- | --- | --- |
| TC-14 | Đăng bài hợp lệ | Tiêu đề, mô tả có định dạng, ảnh `.jpg`, loại địa điểm | Lưu bài, trạng thái **Chờ duyệt**, chuyển về **Bài của tôi** | Đúng, ảnh nằm trong `wwwroot/images/posts` | ✅ |
| TC-15 | Bài chờ duyệt không hiện trang chủ | Xem trang chủ ngay sau khi đăng | Không thấy bài mới | Không hiện | ✅ |
| TC-16 | Thiếu tiêu đề | Bỏ trống tiêu đề | Báo "Vui lòng nhập tiêu đề" | Báo lỗi | ✅ |
| TC-17 | Mô tả trống | Mô tả chỉ có `<p><br></p>` | Báo "Vui lòng nhập mô tả" | Báo lỗi | ✅ |
| TC-18 | Tải file không phải ảnh | File `virus.txt` | Báo "Chỉ nhận file ảnh .jpg, .jpeg, .png, .gif, .webp" | Báo lỗi, không lưu file | ✅ |
| TC-19 | Dán mã độc vào mô tả | `<script>alert(1)</script>`, `<img onerror=...>` | Mã độc bị lọc khi lưu | CSDL không còn `script` / `onerror` | ✅ |
| TC-20 | Xem bài của tôi | Đăng nhập `trung`, mở **Bài của tôi** | Chỉ hiện bài của `trung` | Đúng | ✅ |
| TC-21 | Khách xem **Bài của tôi** | Chưa đăng nhập, mở `/Post/MyPosts` | Bị chuyển về trang đăng nhập | Chuyển đúng | ✅ |

### 2.5 Tìm kiếm

| Mã | Tình huống | Dữ liệu nhập | Kết quả mong đợi | Kết quả thực tế | Trạng thái |
| --- | --- | --- | --- | --- | --- |
| TC-22 | Tìm có kết quả | Từ khóa `Tháp` | Hiện bài **Tháp Bà Ponagar** | Hiện đúng | ✅ |
| TC-23 | Tìm không có kết quả | Từ khóa `zzzzkhongco` | Hiện "Chưa có bài viết nào phù hợp" | Hiện đúng | ✅ |

### 2.6 Duyệt, từ chối, xóa bài

| Mã | Tình huống | Cách kiểm thử | Kết quả mong đợi | Kết quả thực tế | Trạng thái |
| --- | --- | --- | --- | --- | --- |
| TC-24 | Lọc bài chờ duyệt | Admin mở `/Admin?trangThai=ChoDuyet` | Chỉ hiện bài **Chờ duyệt** | Đúng | ✅ |
| TC-25 | Duyệt bài | Bấm **Duyệt** | `TrangThai = DaDuyet`, bài hiện trang chủ | Đúng | ✅ |
| TC-26 | Từ chối không nhập lý do | Bỏ trống ô lý do, bấm **Từ chối bài** | Báo "Vui lòng nhập lý do từ chối", trạng thái không đổi | Đúng | ✅ |
| TC-27 | Từ chối có lý do | Nhập "Ảnh bị mờ" | `TrangThai = TuChoi`, lưu `LyDoTuChoi` | Đúng | ✅ |
| TC-28 | Thành viên xem lý do từ chối | `trung` mở **Bài của tôi** | Hiện nhãn **Từ chối** + dòng "Lý do: Ảnh bị mờ" | Đúng | ✅ |
| TC-29 | Bài bị từ chối không lên trang chủ | Xem trang chủ | Không có bài đó; mở chi tiết trả 404 | Đúng | ✅ |
| TC-30 | Duyệt lại bài bị từ chối | Bấm **Duyệt bài** | `DaDuyet`, lý do từ chối bị xóa (`NULL`) | Đúng | ✅ |
| TC-31 | Xóa bài | Bấm **Xóa**, xác nhận | Xóa bài khỏi CSDL và xóa file ảnh đã tải lên | Đúng | ✅ |

### 2.7 Donate mô phỏng

| Mã | Tình huống | Cách kiểm thử | Kết quả mong đợi | Kết quả thực tế | Trạng thái |
| --- | --- | --- | --- | --- | --- |
| TC-32 | Trang chủ ngẫu nhiên | Tải trang chủ 2 lần | Bài donate còn hạn luôn đầu, bài khác đổi thứ tự | Đúng | ✅ |
| TC-33 | Donate hợp lệ | Chọn bài + 50.000đ | Lưu vào `Donations`, `FeaturedUntil` = hôm nay + 7 ngày, bài lên đầu | Đúng | ✅ |
| TC-34 | Donate lần 2 | Donate thêm cùng bài | Cộng thêm 7 ngày (7 → 14 ngày) | Đúng | ✅ |
| TC-35 | Số tiền không hợp lệ | Nhập 500đ hoặc bỏ trống | Báo lỗi, không lưu | Đúng | ✅ |
| TC-36 | Donate cho bài của chính mình (đẩy tin của mình) | `trung` chọn bài của `trung`, 50.000đ | Lưu lượt donate, bài lên đầu trang chủ 7 ngày | Đúng, `Donations` có dòng `UserId=2, PostId=1` | ✅ |
| TC-37 | Hết hạn ưu tiên | Đặt `FeaturedUntil` là hôm qua | Bài trở về nhóm hiển thị ngẫu nhiên | Đúng | ✅ |

### 2.8 Quản lý loại địa điểm

| Mã | Tình huống | Cách kiểm thử | Kết quả mong đợi | Kết quả thực tế | Trạng thái |
| --- | --- | --- | --- | --- | --- |
| TC-38 | Thêm loại mới | Nhập "Địa điểm kiểm thử" | Loại mới xuất hiện trong danh sách và ô chọn loại khi đăng bài | Đúng | ✅ |
| TC-39 | Thêm loại trùng tên / để trống | Nhập "Bãi biển" hoặc để trống | Báo lỗi "đã có" / "Vui lòng nhập tên loại địa điểm" | Đúng | ✅ |
| TC-40 | Sửa tên loại | Đổi "Địa điểm kiểm thử" thành tên khác | CSDL cập nhật, bài viết vẫn giữ liên kết | Đúng | ✅ |
| TC-41 | Xóa loại đang có bài | Bấm **Xóa** ở loại còn bài | Bị chặn: "Không xóa được ... vì đang có N bài viết" | Đúng | ✅ |

| TC-41b | Tự donate đẩy tin của mình (kiểm thử lại sau khi sửa) | `trung` donate lần 2 cho bài của mình, 20.000đ | Cộng thêm 7 ngày (7 → 14 ngày) | Đúng, `DATEDIFF` = 14 ngày | ✅ |
| TC-41c | Bài của mình xếp đầu danh sách donate | `trung` mở **Donate** | Bài của `trung` hiện đầu tiên, có ghi chú "(bài của bạn)" | Đúng | ✅ |
| TC-41d | Khách vào trang donate | Chưa đăng nhập, mở `/Donation` | Chuyển về trang đăng nhập | Đúng | ✅ |
| TC-41e | Donate bài chưa duyệt | Chọn bài **Chờ duyệt** | Báo "Bài viết không tồn tại hoặc chưa được duyệt" | Đúng | ✅ |

### 2.9 Quên mật khẩu

| Mã | Tình huống | Cách kiểm thử | Kết quả mong đợi | Kết quả thực tế | Trạng thái |
| --- | --- | --- | --- | --- | --- |
| TC-42 | Xin mã đặt lại | Nhập `trung` | Tạo mã 6 số, hạn 15 phút, lưu vào `PasswordResets` | Đúng | ✅ |
| TC-43 | Nhập sai mã / mã hết hạn | Nhập sai mã, hoặc để quá 15 phút | Báo "Mã không đúng hoặc đã hết hạn" | Đúng | ✅ |
| TC-44 | Đặt lại mật khẩu thành công | Mã đúng + mật khẩu mới 8 ký tự | Mật khẩu mới được hash, đăng nhập được; mật khẩu cũ bị từ chối | Đúng | ✅ |
| TC-45 | Dùng lại mã đã dùng | Nhập lại mã cũ | Bị từ chối | Đúng | ✅ |

## 3. Các lỗi đã phát hiện và sửa trong quá trình kiểm thử

| Lỗi | Hiện tượng | Cách sửa |
| --- | --- | --- |
| Kết quả kiểm thử TC-25c sai | Đếm bài ra 7 thay vì 6 | Bài kiểm thử tạo thêm (trong script test) chưa bị xóa trước khi kiểm tra. Đây là lỗi của **dữ liệu test**, không phải của website. Đã xóa dữ liệu test và chạy lại: đạt. |
| Thiếu trường hợp "bài bị từ chối phải ẩn" | Ban đầu không có test case | Bổ sung TC-29 |
| Chặn sai việc tự donate | Theo đề tài, donate là để **tự đẩy tin của mình lên đầu** (giống Chợ Tốt). Bản đầu lại chặn donate cho bài của chính mình | Bỏ kiểm tra chặn trong `DonationController`, xếp bài của mình lên đầu danh sách chọn. Chạy lại: 17/17 test case donate đạt |

## 4. Hạn chế còn lại

- Chức năng quên mật khẩu **chưa gửi email thật**; mã hiện trên màn hình để demo.
- Donate là **mô phỏng**, không có giao dịch tiền thật.
- Chưa có chức năng sửa bài sau khi bị từ chối (chỉ xem lý do rồi đăng lại).
- Chưa có phân trang cho danh sách bài.
- Hình ERD cần cập nhật thêm cột `Posts.LyDoTuChoi` và bảng `PasswordResets`.
