# 🎬 Video Merger Tool - WPF Application

Tool nối video WPF - Gộp nhiều file video thành một tập dài sử dụng FFmpeg

## ✨ Tính Năng

- ✅ **Thêm file video** - Chọn nhiều file video cùng lúc
- ✅ **Thêm từ folder** - Import tất cả video trong một thư mục
- ✅ **Sắp xếp thứ tự** - Di chuyển video lên/xuống trong danh sách
- ✅ **Xóa video** - Loại bỏ video khỏi danh sách
- ✅ **Hỗ trợ nhiều định dạng** - MP4, MKV, AVI, MOV, FLV, WMV
- ✅ **Chọn định dạng output** - MP4, MKV, AVI, MOV
- ✅ **Test FFmpeg** - Kiểm tra FFmpeg đã cài đặt chưa
- ✅ **Progress bar** - Hiển thị tiến độ nối video
- ✅ **Mở thư mục output** - Nhanh chóng truy cập file đã nối

## 📋 Yêu Cầu Hệ Thống

- **Windows** 10 hoặc cao hơn
- **.NET Framework** 6.0 trở lên (Windows Desktop)
- **FFmpeg** (cài đặt và thêm vào PATH)

## 🛠️ Cài Đặt

### 1. Cài FFmpeg

**Cách 1: Dùng Chocolatey (Khuyến Nghị)**
```powershell
# Mở PowerShell as Administrator
choco install ffmpeg
```

**Cách 2: Tải Manual**
1. Vào https://ffmpeg.org/download.html
2. Chọn Windows build
3. Tải file `.zip`
4. Giải nén vào `C:\ffmpeg`
5. Thêm `C:\ffmpeg\bin` vào PATH hoặc chỉ định đường dẫn trong app

**Kiểm tra FFmpeg:**
```cmd
ffmpeg -version
```

### 2. Tạo Project C#

```bash
# Clone repository
git clone https://github.com/khuchatxuainfo-cmd/VideoMergerWPF.git
cd VideoMergerWPF

# Khôi phục dependencies
dotnet restore

# Chạy application
dotnet run
```

## 🚀 Cách Sử Dụng

1. **Thêm Video**
   - Nhấp "Thêm File" để chọn file video từng cái
   - Hoặc "Thêm Folder" để import tất cả video trong thư mục

2. **Sắp Xếp Thứ Tự**
   - Dùng nút "▲" và "▼" để di chuyển video
   - Thứ tự video trong danh sách sẽ là thứ tự nối

3. **Cấu Hình Output**
   - Chọn đường dẫn lưu file output
   - Chọn định dạng video (MP4, MKV, AVI, MOV)
   - Nhập hoặc chọn đường dẫn FFmpeg

4. **Kiểm Tra FFmpeg**
   - Nhấp nút "Test" để xác nhận FFmpeg đã cài đặt

5. **Bắt Đầu Nối**
   - Nhấp "▶️ Bắt Đầu Nối Video"
   - Chờ quá trình hoàn thành
   - Nhấp "Mở Thư Mục Output" để xem file đã nối

## 📝 Lưu Ý

- ⚠️ Đảm bảo tất cả video có **cùng độ phân giải** và **frame rate**
- ⚠️ FFmpeg phải được **cài đặt** hoặc **chỉ định đúng đường dẫn**
- ⚠️ Quá trình nối có thể **mất vài phút** tùy kích thước file
- ⚠️ Nếu video không cùng codec, hãy dùng tùy chọn "Re-encode" (nếu có)

## 🔧 Tấu Trúc Dự Án

```
VideoMergerWPF/
├── MainWindow.xaml           # Giao diện WPF
├── MainWindow.xaml.cs        # Logic xử lý
├── VideoMergerWPF.csproj     # File project
└── README.md                 # Tài liệu
```

## 💻 Stack Công Nghệ

- **C#** (.NET 6.0)
- **WPF** (Windows Presentation Foundation)
- **FFmpeg** (Video processing)
- **Windows Forms** (Folder browser dialog)

## 📄 License

MIT License - Tự do sử dụng cho mục đích thương mại và cá nhân

## 🤝 Đóng Góp

Mọi đóng góp đều được chào đón! Vui lòng:
1. Fork repository
2. Tạo branch mới (`git checkout -b feature/improvement`)
3. Commit thay đổi (`git commit -m 'Add improvement'`)
4. Push lên branch (`git push origin feature/improvement`)
5. Tạo Pull Request

## 📧 Liên Hệ

Có câu hỏi hoặc đề xuất? Mở một [Issue](https://github.com/khuchatxuainfo-cmd/VideoMergerWPF/issues)

---

**Made with ❤️ by khuchatxuainfo-cmd**