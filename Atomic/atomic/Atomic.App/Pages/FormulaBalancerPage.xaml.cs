using Microsoft.Advertising.WinRT.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Atomic.App.Pages
{ 

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FormulaBalancerPage : Page
    {
        InterstitialAd myInterstitialAd = null;
        string myAppId = "9wzdncrdm83q";
        string myAdUnitId = "1100011960";
        int viewCount = 0;

        public FormulaBalancerPage()
        {
            this.InitializeComponent();

            SetUpPageAnimation();

            myInterstitialAd = new InterstitialAd();
            myInterstitialAd.AdReady += MyInterstitialAd_AdReady;
            myInterstitialAd.ErrorOccurred += MyInterstitialAd_ErrorOccurred;
            myInterstitialAd.Completed += MyInterstitialAd_Completed;
            myInterstitialAd.Cancelled += MyInterstitialAd_Cancelled;
            myInterstitialAd.RequestAd(AdType.Display, myAppId, myAdUnitId);
        }

        private void SetUpPageAnimation()
        {
            TransitionCollection collection = new TransitionCollection();
            NavigationThemeTransition theme = new NavigationThemeTransition();

            var info = new ContinuumNavigationTransitionInfo();

            theme.DefaultNavigationTransitionInfo = info;
            collection.Add(theme);
            this.Transitions = collection;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            FormulaTextbox.Text = string.Empty;
            ErrorBlock.Visibility = Visibility.Collapsed;
            SolutionBlock.Visibility = Visibility.Collapsed;
        }

        private void BalanceButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewCount % 5 == 0)
            {
                if (InterstitialAdState.Ready == myInterstitialAd.State)
                {
                    myInterstitialAd.Show();
                }
            }
            viewCount++;

            string formulaText = FormulaTextbox.Text;

            Model.BalancedFormula balancedFormula = Model.FormulaBalancer.Balance(formulaText);

            if( !balancedFormula.IsError )
            {
                ErrorBlock.Visibility = Visibility.Collapsed;
                SolutionBlock.Visibility = Visibility.Visible;
                List<Run> displayText = new List<Run>(balancedFormula.DisplayFormula);
                SolutionTextblock.Inlines.Clear();
                foreach (Run r in displayText)
                {
                    SolutionTextblock.Inlines.Add(r);
                }

                List<Run> stepsFormula = new List<Run>(balancedFormula.StepsFormula);
                StepsFormulaTextblock.Inlines.Clear();
                foreach(Run r in stepsFormula)
                {
                    StepsFormulaTextblock.Inlines.Add(r);
                }

                AMatrixTextblock.Text = balancedFormula.AMatrixDisplay;
                BMatrixTextblock.Text = balancedFormula.BMatrixDisplay;

                FirstMatrixSolvedTextblock.Text = balancedFormula.FirstResultDisplay;
                SecondMatrixSolvedTextblock.Text = balancedFormula.SecondResultDisplay;
                GCDSolvedTextblock.Text = balancedFormula.GCD;

                SolutionStepItems.ItemsSource = balancedFormula.Steps;
            }
            else
            {
                ErrorBlock.Visibility = Visibility.Visible;
                SolutionBlock.Visibility = Visibility.Collapsed;

                ErrorTextblock.Text = balancedFormula.Result;
            }

        }

        private void FormulaTextbox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                BalanceButton_Click(sender, e);
            }
        }

        #region Interstitial Ad Events
        void MyInterstitialAd_AdReady(object sender, object e)
        {
            // Your code goes here.
        }

        void MyInterstitialAd_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            // Your code goes here.
            myInterstitialAd.RequestAd(AdType.Display, myAppId, myAdUnitId);
        }

        void MyInterstitialAd_Completed(object sender, object e)
        {
            myInterstitialAd.RequestAd(AdType.Display, myAppId, myAdUnitId);
        }

        void MyInterstitialAd_Cancelled(object sender, object e)
        {
            // Your code goes here.
        }
        #endregion
    }
}
