using OneDriveUploaderSample.Model.OneDrive;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LoadToolSampleWPF
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public OneDriveService ServiceInstance
        {
            get;
            private set;
        }

        // ここに文字列でアプリケーション IDを指定してください。例:"0000000desak0ui9"
        private const string _ONEDRIVE_APP_ID = //"";

        public App()
        {
            InitializeComponent();

            ServiceInstance = new OneDriveService(_ONEDRIVE_APP_ID);
        }
    }
}
