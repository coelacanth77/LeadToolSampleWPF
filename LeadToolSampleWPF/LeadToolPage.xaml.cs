using Leadtools;
using Leadtools.Codecs;
using Leadtools.ImageProcessing;
using Leadtools.Windows.Media;
using LeadToolSampleWPF.Model.LeadTools;
using OneDriveUploaderSample.Model.OneDrive;
using OneDriveUploaderSample.Model.OneDrive.Response;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using System;

namespace LoadToolSampleWPF
{
    /// <summary>
    /// LeadToolPage.xaml の相互作用ロジック
    /// </summary>
    public partial class LeadToolPage : Page
    {
        private readonly OneDriveService _service = ((App)Application.Current).ServiceInstance;

        private ItemInfoResponse _itemInfo;

        private Stream _originalStream;

        public LeadToolPage()
        {
            InitializeComponent();

            Support.SetLicense();
        }

        // OneDriveから画像を読み込む
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pathTextBox.Text) == true)
            {
                return;
            }

            this._itemInfo = await _service.GetItem(pathTextBox.Text);

            using (this._originalStream = await _service.RefreshAndDownloadContent(this._itemInfo, false))
            {

                if (this._originalStream != null)
                {
                    var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = this._originalStream;
                    imageSource.EndInit();

                    this.image.Source = imageSource;

                    this.changeButton.IsEnabled = true;
                }
            }
                
        }

        // 画像を変換する
        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RasterImage tempImage = RasterImageConverter.ConvertFromSource(this.image.Source, ConvertFromSourceOptions.None);

                // ここではグレイスケールを適応している。
                // その他のエフェクトについてはサンプルのImageProcessingDemoを参照
                // デモはC:\LEADTOOLS 19\Examples\以下にインストールされている
                GrayscaleCommand command = new GrayscaleCommand();
                command.BitsPerPixel = 8;
                command.Run(tempImage);

                this.changedImage.Source = RasterImageConverter.ConvertToSource(tempImage, ConvertToSourceOptions.None); 

                this.uploadButton.IsEnabled = true;
            }
            // 開発用ライセンスを正常に読み込ませていない場合
            // ex.Messageが「Kernel has expired」になる
            // 開発用ライセンスはC:\LEADTOOLS 19\Common\Licenseに配置されている
            catch (RasterException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private async void uploadButton_Click(object sender, RoutedEventArgs e)
        {
            var folder = await _service.GetAppRoot();

            WriteableBitmap bitmap = this.changedImage.Source as WriteableBitmap;

            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                var response = await _service.SaveFile(folder.Id, "test.jpg", stream);
            }
        }
    }
}
