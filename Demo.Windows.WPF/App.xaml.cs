using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Demo.Windows.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        override protected void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Initialize the theme initializer
            Theme.Initializer.Instance.Initialize("Demo.Windows.WPF");
            Current.Resources.MergedDictionaries.Add(Theme.Resources.ThemeResources.LanguageResourceDictionary);
        }
    }

    
}
