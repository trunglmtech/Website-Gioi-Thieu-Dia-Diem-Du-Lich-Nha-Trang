namespace DuLichNhaTrang.Models
{
    // Đổi mã trạng thái trong CSDL thành chữ tiếng Việt để hiển thị
    public static class TrangThaiBaiViet
    {
        public static string TenHienThi(string trangThai)
        {
            switch (trangThai)
            {
                case "DaDuyet":
                    return "Đã duyệt";
                case "TuChoi":
                    return "Từ chối";
                default:
                    return "Chờ duyệt";
            }
        }

        // Tên lớp CSS để tô màu nhãn trạng thái
        public static string LopHienThi(string trangThai)
        {
            switch (trangThai)
            {
                case "DaDuyet":
                    return "nt-status-da";
                case "TuChoi":
                    return "nt-status-tu";
                default:
                    return "nt-status-cho";
            }
        }
    }
}
