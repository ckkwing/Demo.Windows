using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Theme.CustomControl;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using WindowsRTBridge.Share;

namespace Demo.Windows.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseWindow
    {
        public enum ShareMode
        {
            Blank,
            Text,
            Weblink,
            StorageItem
        }

        ShareMode currentShareMode = ShareMode.Blank;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            var dtm = DataTransferManagerHelper.GetForWindow(hwnd);
            dtm.DataRequested += OnDataRequested;
        }

        async void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            Trace.WriteLine("OnDataRequested start");
            var deferral = args.Request.GetDeferral();
            string title = "Title test";
            string text = "Text test";
            string description = "Description test";
            string link = "https://baidu.com";
            string appName = "Application Name";
            try
            {
                DataPackage dp = args.Request.Data;
                dp.Properties.Title = title;
                dp.Properties.Description = description;
                dp.Properties.ApplicationName = appName;
                dp.Properties.ApplicationListingUri = new Uri(@"ms-windows-store://pdp/?productid=9N6KNQC4FXZC");

                switch (currentShareMode)
                {
                    case ShareMode.Text:
                        dp.SetText(text);
                        break;

                    case ShareMode.Weblink:
                        dp.SetWebLink(new System.Uri(link));
                        break;

                    case ShareMode.StorageItem:
                        var filesToShare = new List<IStorageItem>();
                        StorageFile imageFile = await StorageFile.GetFileFromPathAsync(AppDomain.CurrentDomain.BaseDirectory + "Images\\image.jpg");
                        filesToShare.Add(imageFile);

                        dp.SetStorageItems(filesToShare);
                        break;
                }
            }
            finally
            {
                deferral.Complete();
            }
            Trace.WriteLine("OnDataRequested end");
        }

        private void ShowShareUI()
        {
            IntPtr hwnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            DataTransferManagerHelper.ShowShareUIForWindow(hwnd);
        }

        private void btnShare_Click(object sender, RoutedEventArgs e)
        {
            currentShareMode = ShareMode.Weblink;
            ShowShareUI();
        }

        private void btnUri_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
