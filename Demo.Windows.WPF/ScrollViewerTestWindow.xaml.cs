using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using Theme.Command;
using Theme.CustomControl;

namespace Demo.Windows.WPF
{
    /// <summary>
    /// Interaction logic for ScrollViewerTestWindow.xaml
    /// </summary>
    public partial class ScrollViewerTestWindow : ChromeBaseWindow
    {
        //ObservableCollection<int> items = new ObservableCollection<int>();
        //public ObservableCollection<int> Items { get => items; set => items = value; }

        private ICommand _reachedBottomCommand;
        public ICommand ReachedBottomCommand
        {
            get
            {
                _reachedBottomCommand = _reachedBottomCommand ?? new GenericCommand()
                {
                    CanExecuteCallback = (parameter) => true,
                    ExecuteCallback = OnReachedBottomCommand
                };

                return _reachedBottomCommand;
            }
        }


        public ScrollViewerTestWindow()
        {
            InitializeComponent();
            DataContext = this;

        }

        private void ChromeBaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                //Items.Add(i);
                mainPanel.Children.Add(new TextBlock() { Text = i.ToString() });
            }
        }

        private void OnReachedBottomCommand(object obj)
        {
            MessageBox.Show("Scroll viewer reached bottom");
        }

    }
}
