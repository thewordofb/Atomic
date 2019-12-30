using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Store;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Atomic.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private StoreContext context = null;

        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SetTitleBar(TitleBarBlock);

            if (!Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported())
            {
                this.feedbackButton.Visibility = Visibility.Collapsed;
            }

            GetLicenseInfo();

          //  var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
          //  coreTitleBar.ExtendViewIntoTitleBar = true;

            // Set XAML element as a draggable region.
            //AppTitleBar.Height = coreTitleBar.Height;

            HamburgerMenu.SelectedIndex = 0;
            ContentFrame.Navigate(typeof(Atomic.App.Pages.PeriodicTablePage));
        }

        private async void reviewButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + Windows.ApplicationModel.Store.CurrentApp.AppId));
        }

        private async void feedbackButton_Click(object sender, RoutedEventArgs e)
        {
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }

        private void TableButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(Atomic.App.Pages.PeriodicTablePage));
        }

        private void BalanceButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(Atomic.App.Pages.FormulaBalancerPage));
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(Atomic.App.Pages.AboutPage));
        }

        private void StoichiometryButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(Atomic.App.Pages.StoichiometryPage));
        }

        private void EmpiricalFormulaButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(Atomic.App.Pages.EmpiricalFormulaPage));
        }

        private void IdealGasButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(Atomic.App.Pages.IdealGasPage));
        }

        private void LimitingReagentButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(Atomic.App.Pages.LimitingReagentPage));
        }

        private void MolarMassButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(Atomic.App.Pages.MolarMassPage));
        }

        private void HamburgerMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            HamburgerMenu.IsPaneOpen = false;
            if (e.ClickedItem == BalanceEqItem)
            {
                BalanceButton_Click(sender, e);
            }
            else if (e.ClickedItem == TableItem)
            {
                TableButton_Click(sender, e);
            }
            else if (e.ClickedItem == StoichItem)
            {
                StoichiometryButton_Click(sender, e);
            }
            else if(e.ClickedItem == EmpiricalForm)
            {
                EmpiricalFormulaButton_Click(sender, e);
            }
            else if (e.ClickedItem == IdealGas)
            {
                IdealGasButton_Click(sender, e);
            }
            else if (e.ClickedItem == LimitingReagent)
            {
                LimitingReagentButton_Click(sender, e);
            }
            else if (e.ClickedItem == MolarMass)
            {
                MolarMassButton_Click(sender, e);
            }
        }

        private void HamburgerMenu_OptionsItemClick(object sender, ItemClickEventArgs e)
        {
            HamburgerMenu.IsPaneOpen = false;
            AboutButton_Click(sender, e);
        }

        private void AdFreeHyperlink_Click(object sender, RoutedEventArgs e)
        {
            PurchaseAddOn("9pjcvfq6406s");
        }

        public async void GetLicenseInfo()
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }

            StoreAppLicense appLicense = await context.GetAppLicenseAsync();

            if (appLicense == null)
            {
                // textBlock.Text = "An error occurred while retrieving the license.";
                return;
            }

            // Access the add on licenses for add-ons for this app.
            foreach (KeyValuePair<string, StoreLicense> item in appLicense.AddOnLicenses)
            {
                StoreLicense addOnLicense = item.Value;
                // Use members of the addOnLicense object to access license info
                // for the add-on...
                //addOnLicense.
                if (addOnLicense.IsActive)
                {
                    AdGrid.Visibility = Visibility.Collapsed;
                }
            }
        }

        public async void PurchaseAddOn(string storeId)
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }

            StorePurchaseResult result = await context.RequestPurchaseAsync(storeId);

            // Capture the error message for the operation, if any.
            string extendedError = string.Empty;
            if (result.ExtendedError != null)
            {
                extendedError = result.ExtendedError.Message;
            }

            switch (result.Status)
            {
                case StorePurchaseStatus.AlreadyPurchased:
                    AdGrid.Visibility = Visibility.Collapsed;
                    break;

                case StorePurchaseStatus.Succeeded:
                    AdGrid.Visibility = Visibility.Collapsed;
                    break;

                case StorePurchaseStatus.NotPurchased:
                    //     textBlock.Text = "The purchase did not complete. " +
                    //          "The user may have cancelled the purchase. ExtendedError: " + extendedError;
                    break;

                case StorePurchaseStatus.NetworkError:
                    //      textBlock.Text = "The purchase was unsuccessful due to a network error. " +
                    //          "ExtendedError: " + extendedError;
                    break;

                case StorePurchaseStatus.ServerError:
                    //      textBlock.Text = "The purchase was unsuccessful due to a server error. " +
                    //          "ExtendedError: " + extendedError;
                    break;

                default:
                    //      textBlock.Text = "The purchase was unsuccessful due to an unknown error. " +
                    //          "ExtendedError: " + extendedError;
                    break;
            }
        }


    }
}
