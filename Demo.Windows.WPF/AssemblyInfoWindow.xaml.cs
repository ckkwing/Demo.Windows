using CommonUtility.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Theme.CustomControl;

namespace Demo.Windows.WPF
{
    /// <summary>
    /// Interaction logic for AssemblyInfoWindow.xaml
    /// </summary>
    public partial class AssemblyInfoWindow : ChromeBaseWindow
    {
        private string assemblyPath = @"D:\Projects\Stash\3.app\nerodvdplayer\src\bin\Release\cdemu.exe";
        public  string AssemblyPath
        {
            get => assemblyPath;
            set
            {
                assemblyPath = value;
                RaisePropertyChanged(nameof(AssemblyPath));
            }
        }

        private string assemblyInfo = string.Empty;
        public string AssemblyInfo
        {
            get => assemblyInfo;
            set
            {
                assemblyInfo = value;
                RaisePropertyChanged(nameof(AssemblyInfo));
            }
        }

        public AssemblyInfoWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            AssemblyInfo = string.Empty;
            AssemblyInfo += FileSystemUtility.GetAssemblyInfo(AssemblyPath);
            AssemblyInfo += FileSystemUtility.GetFileVersionInfo(AssemblyPath);
        }

        
    }
}
