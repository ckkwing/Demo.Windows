using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for PathTestWindows.xaml
    /// </summary>
    public partial class PathTestWindows : ChromeBaseWindow, INotifyPropertyChanged
    {
        private int _progressBarValue = 0;
        public int ProgressBarValue
        {
            get => _progressBarValue;
            set
            {
                _progressBarValue = value;
                OnPropertyChanged(nameof(ProgressBarValue));
            }
        }

        public Point ProgressPoint
        {
            get
            {
                double angle = 360 * (ProgressBarValue / 100.0); // 计算当前角度
                double radians = (angle - 90) * (Math.PI / 180); // 转换为弧度并调整起点
                double x = 50 + 50 * Math.Cos(radians);
                double y = 50 + 50 * Math.Sin(radians);
                return new Point(x, y);
            }
        }

        public bool IsLargeArc => ProgressBarValue > 50; // 超过半圆时使用大弧

        public PathTestWindows()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnReset.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, this));
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ProgressBarValue = 0;
            Task.Run(() => {
                while (ProgressBarValue < 100)
                {
                    ProgressBarValue += 1;
                    Thread.Sleep(100);
                }
            });
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
