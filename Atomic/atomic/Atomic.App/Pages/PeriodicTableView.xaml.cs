using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Atomic.Elements;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Atomic.Pages
{
    public sealed partial class PeriodicTableView : UserControl
    {
        public PeriodicTableView()
        {
            // DataContext = this;

            this.InitializeComponent();
        }

        private void ElementClicked(object sender, RoutedEventArgs e)
        {
            var myValue = ((Button)sender).Tag;

            TableElements.SelectedElement = (ChemElement)myValue;


            ElementDetailsPopup.IsOpen = true;
            /*Flyout ElementDetailsFlyout = new Flyout();
            ElementDetailsFlyout.Content = new Border() { Width = 500, Height = 500 };

            ElementDetailsFlyout.Placement = PlacementMode.Top;
            ElementDetailsFlyout.PlacementTarget = (Button)sender; // this is an UI element (usually the sender)

            ElementDetailsFlyout.IsOpen = true;
            */


        }

        public PeriodicTable TableElements
        {
            get { return (PeriodicTable)GetValue(TableElementsProperty); }
            set { SetValue(TableElementsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TableElements.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TableElementsProperty =
            DependencyProperty.Register("TableElements", typeof(PeriodicTable), typeof(PeriodicTableView), new PropertyMetadata(new PeriodicTable(), new PropertyChangedCallback(OnElementsChanged)));

        private static void OnElementsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PeriodicTableView view = (PeriodicTableView)d;
            PeriodicTable table = (PeriodicTable)e.NewValue;
            foreach (ChemElement ce in table.Elements)
            {
                ContentPresenter cp = new ContentPresenter();
                cp.ContentTemplate = (DataTemplate)view.Resources["ElementTemplate"];
                cp.Content = ce;
                Grid.SetRow(cp, ce.Row);
                Grid.SetColumn(cp, ce.Column);

                view.PTableGrid.Children.Add(cp);
            }
        }

        private void PopupDismissButton_Click(object sender, RoutedEventArgs e)
        {
            ElementDetailsPopup.IsOpen = false;
        }
    }

    public class UnitsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NoneTemplate { get; set; }
        public DataTemplate CelsiusTemplate { get; set; }
        public DataTemplate AngstromTemplate { get; set; }
        public DataTemplate CubicCentimeterPerMoleTemplate { get; set; }
        public DataTemplate GramsPerLiterTemplate { get; set; }
        public DataTemplate PaulingTemplate { get; set; }
        public DataTemplate JoulesPerKelvinTemplate { get; set; }
        public DataTemplate KiloJoulesPerMoleTemplate { get; set; }
        public DataTemplate WattsPerMeterKelvinTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            DataTemplate dt = NoneTemplate;

            if (item is Celsius)
            {
                dt = CelsiusTemplate;
            }
            else if (item is Angstrom)
            {
                dt = AngstromTemplate;
            }
            else if (item is CubicCentimeterPerMole)
            {
                dt = CubicCentimeterPerMoleTemplate;
            }
            else if (item is GramsPerLiter)
            {
                dt = GramsPerLiterTemplate;
            }
            else if (item is Pauling)
            {
                dt = PaulingTemplate;
            }
            else if (item is JoulesPerKelvin)
            {
                dt = JoulesPerKelvinTemplate;
            }
            else if (item is KiloJoulesPerMole)
            {
                dt = KiloJoulesPerMoleTemplate;
            }
            else if (item is WattsPerMeterKelvin)
            {
                dt = WattsPerMeterKelvinTemplate;
            }

            return dt;
        }
    }
}
