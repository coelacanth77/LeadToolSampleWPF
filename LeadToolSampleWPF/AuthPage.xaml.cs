using LoadToolSampleWPF;
using OneDriveUploaderSample.Model.OneDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LeadToolSampleWPF
{
    /// <summary>
    /// AuthPage.xaml の相互作用ロジック
    /// </summary>
    public partial class AuthPage : Page
    {
        private readonly OneDriveService _service = ((App)Application.Current).ServiceInstance;

        public AuthPage()
        {
            InitializeComponent();

            _service = ((App)Application.Current).ServiceInstance;
            if (!_service.CheckAuthenticate(
                () =>
                {
                    MessageBoxResult result = MessageBox.Show("認証に成功しました。", "認証結果", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new LeadToolPage());
                },
                () =>
                {
                    MessageBoxResult result = MessageBox.Show("認証に失敗しました。", "認証結果", MessageBoxButton.OK, MessageBoxImage.Warning);
                }))
            {
            }

            this.webBrowser.Source = _service.GetStartUri();

            webBrowser.Navigated += WebBrowser_Navigated;
        }

        private void WebBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            if (_service.CheckRedirectUrl(e.Uri.AbsoluteUri))
            {
                _service.ContinueGetTokens(e.Uri);
            }
        }
    }
}
