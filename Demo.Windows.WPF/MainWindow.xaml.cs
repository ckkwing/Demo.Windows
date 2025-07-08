using CommonUtility.Encryption;
using CommonUtility.Helper;
using CommonUtility.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
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

namespace Demo.Test1
{
    [DataContract(Namespace = "http://schemas.example.com")]
    public class AAA
    {
        [DataMember]
        public string Name { get; set; }
    }
}

namespace Demo.Test2
{
    [DataContract(Namespace = "http://schemas.example.com")]
    public class AAA
    {
        [DataMember]
        public string Name { get; set; }
    }
}

namespace Demo.Windows.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ChromeBaseWindow, INotifyPropertyChanged
    {
        public enum ShareMode
        {
            Blank,
            Text,
            Weblink,
            StorageItem
        }

        ShareMode currentShareMode = ShareMode.Blank;
        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            var dtm = DataTransferManagerHelper.GetForWindow(hwnd);
            dtm.DataRequested += OnDataRequested;

            CommonUtility.Logger.LogHelper.DefaultLogger.Debug("Window_Loaded");

            //Test
            try
            {
                string fileName = @"E:\Temp\1.test";
                EncryptionHelper.SerializeObjectToFile(fileName, typeof(Test1.AAA), null, new Test1.AAA() { Name = "ckk" }, "NeroAG20230721~!", "@#20230721NeroAG");
                var a1 = EncryptionHelper.DeserializeObjectFromFile(fileName, typeof(Test1.AAA), null, "NeroAG20230721~!", "@#20230721NeroAG") as Test1.AAA;
                var a2 = EncryptionHelper.DeserializeObjectFromFile(fileName, typeof(Test2.AAA), null, "NeroAG20230721~!", "@#20230721NeroAG") as Test2.AAA;
            }
            catch (Exception ex)
            {
            }
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

        private void btnLoadingAnimation_Click(object sender, RoutedEventArgs e)
        {
            LoadingAnimationWindow animationWindow = new LoadingAnimationWindow();
            animationWindow.ShowDialog();
        }

        private void btnCurrencyTest_Click(object sender, RoutedEventArgs e)
        {
            string machineCurrentLocation = Utility.WindowsSystem.GetMachineCurrentLocation();

            var currentRegionInfo = RegionInfo.CurrentRegion;

            decimal price;
            string iso4217CurrencyCode = "USD";
            CultureInfo cultureInfo = CurrencyHelper.GetCultureInfoCollection(iso4217CurrencyCode).FirstOrDefault() ?? CultureInfo.CurrentCulture; //new CultureInfo("fr-FR");
            //NumberFormatInfo.CurrentInfo is equal to cultureInfo
            if (CurrencyHelper.TryParseCurrency("$45.00", cultureInfo, out price))
            {
                string symbol = string.Empty;
                CurrencyHelper.TryGetCurrencySymbol(iso4217CurrencyCode, out symbol);
                MessageBox.Show($"Price: {CurrencyHelper.ToCurrency(price, cultureInfo)}, Symbol: {symbol}");
            }
            else
            {
                MessageBox.Show("Converter failed");
            }
        }

        private void btnImageTest_Click(object sender, RoutedEventArgs e)
        {
            ImageTestWindow imageTestWindow = new ImageTestWindow();
            imageTestWindow.ShowDialog();
        }

        private void btnStartNetworkAdapterWatcher_Click(object sender, RoutedEventArgs e)
        {
            NetworkHelper.StartNetworkAdapterWatcher();
        }

        private void btnStopNetworkAdapterWatcher_Click(object sender, RoutedEventArgs e)
        {
            NetworkHelper.StopNetworkAdapterWatcher();
        }

        private void btnStartMonitoringNetworkChanges_Click(object sender, RoutedEventArgs e)
        {
            NetworkHelper.StartMonitoringNetworkChanges();
        }

        private void btnStopMonitoringNetworkChanges_Click(object sender, RoutedEventArgs e)
        {
            NetworkHelper.StopMonitoringNetworkChanges();
        }

        private void btnScrollViewerTest_Click(object sender, RoutedEventArgs e)
        {
            ScrollViewerTestWindow scrollViewerTestWindow = new ScrollViewerTestWindow();
            scrollViewerTestWindow.ShowDialog();
        }

        private void btnSHA256Test_Click(object sender, RoutedEventArgs e)
        {
            string password = "asdfsadfasdf!21312";
            var salt = SHA256Helper.GenerateSalt();
            string saltedHash = SHA256Helper.ComputeSaltedHash(password, salt);
            MessageBox.Show($"password:{password} is {SHA256Helper.VerifyContent(password, saltedHash,salt)}");
            password += "asfasdf";
            MessageBox.Show($"password:{password} is {SHA256Helper.VerifyContent(password, saltedHash, salt)}");
        }

        private void btnPathTest_Click(object sender, RoutedEventArgs e)
        {
            PathTestWindows pathTestWindows = new PathTestWindows();
            pathTestWindows.ShowDialog();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(params string[] properties)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                foreach (string property in properties)
                {
                    propertyChanged(this, new PropertyChangedEventArgs(property));
                }
            }
        }

        #endregion


    }
}
