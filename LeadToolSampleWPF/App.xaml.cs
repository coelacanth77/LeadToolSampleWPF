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

        // OneDriveのClient IDを記述してください。
        private const string _ONEDRIVE_APP_ID = //"";

        public App()
        {
            InitializeComponent();

            ServiceInstance = new OneDriveService(_ONEDRIVE_APP_ID);
        }
    }
}
