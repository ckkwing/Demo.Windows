using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Deom.Windows.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DataTransferManager dataTransferManager;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // Register the current page as a share source.
            this.dataTransferManager = DataTransferManager.GetForCurrentView();
            this.dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);

            // Request to be notified when the user chooses a share target app.
            this.dataTransferManager.TargetApplicationChosen += OnTargetApplicationChosen;
        }

        private void OnTargetApplicationChosen(DataTransferManager sender, TargetApplicationChosenEventArgs e)
        {

        }

        private string title = "Title Test";
        private string description = "Description Test";
        private string text = "Text Test";
        private string link = @"ms-windows-store://pdp/?productid=9N6KNQC4FXZC";
        private bool GetShareContent(DataRequest request)
        {
            bool succeeded = false;

            // The URI used in this sample is provided by the user so we need to ensure it's a well formatted absolute URI
            // before we try to share it.
            Uri dataPackageUri = new Uri(link);
            if (dataPackageUri != null)
            {
                DataPackage requestData = request.Data;
                requestData.Properties.Title = title;
                requestData.Properties.Description = description; // The description is optional.
                requestData.Properties.ContentSourceApplicationLink = dataPackageUri;
                requestData.SetWebLink(dataPackageUri);
                requestData.SetApplicationLink(dataPackageUri);
                requestData.SetText(text);
                succeeded = true;
            }
            else
            {
                request.FailWithDisplayText("Enter the web link you would like to share and try again.");
            }
            return succeeded;
        }

        // When share is invoked (by the user or programatically) the event handler we registered will be called to populate the datapackage with the
        // data to be shared.
        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            // Register to be notified if the share operation completes.
            e.Request.Data.ShareCompleted += OnShareCompleted;

            // Call the scenario specific function to populate the datapackage with the data to be shared.
            if (GetShareContent(e.Request))
            {
                // Out of the datapackage properties, the title is required. If the scenario completed successfully, we need
                // to make sure the title is valid since the sample scenario gets the title from the user.
                if (String.IsNullOrEmpty(e.Request.Data.Properties.Title))
                {
                    e.Request.FailWithDisplayText("Enter a title for what you are sharing and try again.");
                }
            }
        }

        private void OnShareCompleted(DataPackage sender, ShareCompletedEventArgs e)
        {
            string shareCompletedStatus = "Shared successfully. ";

            // Typically, this information is not displayed to the user because the
            // user already knows which share target was selected.
            if (!String.IsNullOrEmpty(e.ShareTarget.AppUserModelId))
            {
                // The user picked an app.
                shareCompletedStatus += $"Target: App \"{e.ShareTarget.AppUserModelId}\"";
            }
            else if (e.ShareTarget.ShareProvider != null)
            {
                // The user picked a ShareProvider.
                shareCompletedStatus += $"Target: Share Provider \"{e.ShareTarget.ShareProvider.Title}\"";
            }

            //this.rootPage.NotifyUser(shareCompletedStatus, NotifyType.StatusMessage);
        }

        private void btnShare_Click(object sender, RoutedEventArgs e)
        {
            // If the user clicks the share button, invoke the share flow programatically.
            DataTransferManager.ShowShareUI();
        }

        private void btnCreateDesktopShortcut_Click(object sender, RoutedEventArgs e)
        {
            //Crash

            //IShellLink link = (IShellLink)new ShellLink();
            //// setup shortcut information
            //link.SetDescription("This is the description when hovered over.");
            //link.SetPath(@"""D:\Stash\nerowindowsnotification\src\UITestbed\bin\Debug\UITestbed.exe""");
            //// save it
            //IPersistFile file = (IPersistFile)link;
            //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            //file.Save(Path.Combine(desktopPath, "Shortcut Display Name.lnk"), false);


        }
    }
}
