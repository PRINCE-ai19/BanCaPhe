1. Kiến trúc dự án
Dự án được xây dựng theo mô hình MVVM (Model-View-ViewModel) chuẩn cho ứng dụng desktop trên nền tảng .NET:

View (Giao diện): Sử dụng WPF (XAML). Các file giao diện nằm ở thư mục gốc và thư mục Views (ví dụ: MainWindow.xaml, AdminDashboard.xaml, W_DangNhap.xaml).
ViewModel (Logic giao diện): Nằm trong thư mục ViewModel. Dự án có sử dụng BaseViewModel để hỗ trợ binding (INotifyPropertyChanged) và RelayCommand để xử lý sự kiện từ UI.
Model (Dữ liệu): Nằm trong thư mục Models, định nghĩa các đối tượng như Đồ uống, Đơn hàng, Nhân viên...
Services (Tầng nghiệp vụ): Thư mục Services chứa các logic xử lý dữ liệu và kết nối cơ sở dữ liệu, giúp tách biệt logic khỏi ViewModel.

2. Công nghệ và Thư viện sử dụng
Dự án chạy trên .NET 8.0-windows với các thư viện chính sau:

Dapper (v2.1.66): Thư viện ORM nhẹ, tốc độ cao. Dựa vào số lượng Store Procedure lớn (39 store), có vẻ bạn đang dùng Dapper để gọi các SQL Store cho tối ưu hiệu năng.
Entity Framework Core (v8.0.17): Bộ ORM mạnh mẽ của Microsoft, dùng để quản lý schema và thao tác dữ liệu.
QuestPDF (v2025.12.3): Thư viện hiện đại dùng để xuất file PDF (khả năng cao là dùng để in hóa đơn thanh toán).
System.Data.SqlClient: Trình điều khiển kết nối đến SQL Server.

3. Các chức năng chính (Dựa trên các file hiện có)
Quản lý bán hàng: Thanh toán tiền mặt, tính tiền, tạm tính hóa đơn (ThanhToanViewModel, HoaDonTamViewModel).
Quản lý kho/Menu: Quản lý sản phẩm, loại đồ uống, kích thước (size) và topping (UC_SanPham, W_LoaiSanPham, UC_AdminTopping).
Quản lý nhân sự: Đăng ký, đăng nhập và quản lý nhân viên (W_DangNhap, W_DangKy, UC_NhanVien).
Báo cáo: Thống kê doanh thu theo thời gian (UC_DoanhThu, DoanhThuViewModel).
Dashboard: Giao diện quản trị tổng thể cho Admin (AdminDashboard).

---

## 4. Giải thích mô hình MVVM cho Intern (Senior's Perspective)

Chào em, để hiểu nhanh về MVVM trong dự án này, em hãy tưởng tượng việc quản lý một quán cà phê thực tế:

### 1. Model (Thế giới thực của dữ liệu)
Là những "bản vẽ" định nghĩa đối tượng. Ví dụ file [DoUong.cs](file:///d:/NET_laptrinh/BanCaPhe/BanCaPhe/Models/DoUong.cs).
- Nó chỉ chứa thuộc tính: `TenDoUong`, `Gia`, `HinhAnh`...
- **Quy tắc**: Model không quan tâm giao diện trông như thế nào, cũng không biết làm sao để lưu vào database.

### 2. View (Giao diện hiển thị)
Là những gì khách hàng (người dùng) nhìn thấy, ví dụ [AdminDashboard.xaml](file:///d:/NET_laptrinh/BanCaPhe/BanCaPhe/AdminDashboard.xaml).
- Viết bằng XAML.
- **Quy tắc**: View không chứa logic tính toán. Nó chỉ "hứng" dữ liệu từ ViewModel để hiển thị và gửi lệnh (Command) khi người dùng bấm nút.

### 3. ViewModel (Bộ não điều phối)
Đây là nơi quan trọng nhất, ví dụ [AdminDashboardViewModel.cs](file:///d:/NET_laptrinh/BanCaPhe/BanCaPhe/ViewModel/AdminDashboardViewModel%20.cs).
- Nó đứng giữa View và Model.
- Nó gọi **Services** để lấy dữ liệu, sau đó "đóng gói" dữ liệu đó vào các thuộc tính (Properties) để View hiển thị.
- Khi em thấy hàm `OnPropertyChanged()`, đó là lúc ViewModel báo cho View: "Dữ liệu đã đổi rồi, cập nhật lên màn hình đi!".

### 4. Services & StoreHelper (Cánh tay thực thi)
Dự án mình có thêm tầng này để ViewModel không bị quá nặng.
- **Service** (ví dụ [DoUongService.cs](file:///d:/NET_laptrinh/BanCaPhe/BanCaPhe/Services/DoUongService.cs)): Chứa logic nghiệp vụ liên quan đến database.
- **StoreHelper**: Là công cụ mình vừa tối ưu hóa, giúp gọi các Stored Procedure từ SQL Server một cách cực kỳ nhanh và gọn bằng Dapper.

---

### Luồng đi của dữ liệu (Ví dụ: Hiển thị Tổng Doanh Thu trên Dashboard)

1.  **Bắt đầu**: Người dùng mở màn hình Dashboard.
2.  **ViewModel**: `AdminDashboardViewModel` được khởi tạo, nó gọi đến `_doanhThuService.GetDoanhThuThangNay()`.
3.  **Service**: `DoanhThuService` dùng `StoreHelper` gọi Store `SP_GetDoanhThuThangNay` dưới database SQL Server.
4.  **Dữ liệu về**: Dapper tự động map kết quả từ SQL thành đối tượng `DoanhThuThangModel`.
5.  **Cập nhật UI**: ViewModel nhận kết quả, gán vào thuộc tính `DoanhThuDisplay`. Lệnh `OnPropertyChanged()` được kích hoạt, màn hình Dashboard tự động hiện số tiền mà không cần load lại trang.

> [!TIP]
> **Lời khuyên từ Senior**: Đừng bao giờ viết code xử lý database trực tiếp trong file `.xaml.cs` (Code-behind). Hãy đưa nó vào **Service**, gọi qua **ViewModel**, và bind lên **View**. Đó là cách giữ cho code của em sạch sẽ, dễ test và dễ mở rộng.
