using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace VideoMergerWPF
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<VideoItem> videoList;
        private bool isMerging = false;

        public MainWindow()
        {
            InitializeComponent();
            videoList = new ObservableCollection<VideoItem>();
            VideoListBox.ItemsSource = videoList;
        }

        // ==================== Event Handlers ====================

        private void AddVideoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Chọn File Video",
                Filter = "Video Files (*.mp4;*.avi;*.mkv;*.mov;*.flv;*.wmv)|*.mp4;*.avi;*.mkv;*.mov;*.flv;*.wmv|All Files (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    if (!videoList.Any(v => v.FilePath == file))
                    {
                        videoList.Add(new VideoItem
                        {
                            FileName = Path.GetFileName(file),
                            FilePath = file
                        });
                    }
                }
                UpdateVideoCount();
            }
        }

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Chọn Thư Mục Chứa Video"
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folderPath = dialog.SelectedPath;
                string[] videoExtensions = { "*.mp4", "*.avi", "*.mkv", "*.mov", "*.flv", "*.wmv" };

                foreach (string ext in videoExtensions)
                {
                    var files = Directory.GetFiles(folderPath, ext).OrderBy(f => f);
                    foreach (string file in files)
                    {
                        if (!videoList.Any(v => v.FilePath == file))
                        {
                            videoList.Add(new VideoItem
                            {
                                FileName = Path.GetFileName(file),
                                FilePath = file
                            });
                        }
                    }
                }
                UpdateVideoCount();
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is VideoItem item)
            {
                videoList.Remove(item);
                UpdateVideoCount();
            }
        }

        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is VideoItem item)
            {
                int index = videoList.IndexOf(item);
                if (index > 0)
                {
                    videoList.Move(index, index - 1);
                }
            }
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is VideoItem item)
            {
                int index = videoList.IndexOf(item);
                if (index < videoList.Count - 1)
                {
                    videoList.Move(index, index + 1);
                }
            }
        }

        private void BrowseOutputButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Chọn Vị Trí Lưu Output",
                Filter = "MP4 Files (*.mp4)|*.mp4|MKV Files (*.mkv)|*.mkv|AVI Files (*.avi)|*.avi|MOV Files (*.mov)|*.mov|All Files (*.*)|*.*",
                DefaultExt = ".mp4"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                OutputPathTextBox.Text = saveFileDialog.FileName;
            }
        }

        private void TestFFmpegButton_Click(object sender, RoutedEventArgs e)
        {
            string ffmpegPath = FFmpegPathTextBox.Text;
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = "-version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                    if (process.ExitCode == 0)
                    {
                        MessageBox.Show("✅ FFmpeg đã được cài đặt đúng!", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("❌ Không tìm thấy FFmpeg!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Xóa tất cả video?", "Xác Nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                videoList.Clear();
                UpdateVideoCount();
            }
        }

        private async void MergeButton_Click(object sender, RoutedEventArgs e)
        {
            if (isMerging)
            {
                MessageBox.Show("Đang nối video, vui lòng chờ!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (videoList.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất 1 file video!", "Cảnh Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (videoList.Count == 1)
            {
                MessageBox.Show("Vui lòng thêm ít nhất 2 file video để nối!", "Cảnh Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            isMerging = true;
            string outputPath = OutputPathTextBox.Text;
            string ffmpegPath = FFmpegPathTextBox.Text;

            StatusLabel.Text = "⏳ Đang nối video...";
            StatusLabel.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 152, 0));
            ProgressBar.Value = 0;
            ProgressTextBlock.Text = "0%";

            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    MergeVideos(ffmpegPath, outputPath);
                });

                MessageBox.Show($"✅ Thành công! File output: {outputPath}", "Hoàn Thành", MessageBoxButton.OK, MessageBoxImage.Information);
                StatusLabel.Text = "✅ Hoàn thành";
                StatusLabel.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(67, 160, 71));
                ProgressBar.Value = 100;
                ProgressTextBlock.Text = "100%";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusLabel.Text = "❌ Có lỗi";
                StatusLabel.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(244, 67, 54));
            }
            finally
            {
                isMerging = false;
            }
        }

        private void OpenOutputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            string outputPath = OutputPathTextBox.Text;
            if (!string.IsNullOrEmpty(outputPath))
            {
                string directory = Path.GetDirectoryName(outputPath);
                if (Directory.Exists(directory))
                {
                    Process.Start("explorer.exe", directory);
                }
                else
                {
                    MessageBox.Show("Thư mục không tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ==================== Helper Methods ====================

        private void MergeVideos(string ffmpegPath, string outputPath)
        {
            string listFile = Path.Combine(Path.GetTempPath(), "videos_list.txt");

            try
            {
                // Tạo file list
                using (StreamWriter writer = new StreamWriter(listFile))
                {
                    foreach (var video in videoList)
                    {
                        string fullPath = Path.GetFullPath(video.FilePath);
                        writer.WriteLine($"file '{fullPath}'");
                    }
                }

                // Lệnh FFmpeg
                string ffmpegArgs = $"-f concat -safe 0 -i \"{listFile}\" -c copy \"{outputPath}\"";

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = ffmpegArgs,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    // Đọc output để cập nhật progress
                    string output = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new Exception("FFmpeg xảy ra lỗi!");
                    }

                    Dispatcher.Invoke(() =>
                    {
                        ProgressBar.Value = 100;
                        ProgressTextBlock.Text = "100%";
                    });
                }
            }
            finally
            {
                if (File.Exists(listFile))
                    File.Delete(listFile);
            }
        }

        private void UpdateVideoCount()
        {
            VideoCountLabel.Text = $"{videoList.Count} file được chọn";
        }
    }

    public class VideoItem
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}