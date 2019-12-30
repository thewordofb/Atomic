using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace metrofoundry.controls
{
    public enum SearchMode
    {

        Instant,
        Delayed,
    }

    public class SearchTextBox : TextBox
    {
        #region Dependency Properties
        public static DependencyProperty LabelTextProperty =
            DependencyProperty.Register(
                "LabelText",
                typeof(string),
                typeof(SearchTextBox),
                new PropertyMetadata("defaultLabelText"));

        public static DependencyProperty LabelTextColorProperty =
            DependencyProperty.Register(
                "LabelTextColor",
                typeof(Brush),
                typeof(SearchTextBox),
                new PropertyMetadata(new SolidColorBrush(Colors.DodgerBlue)));

        public static DependencyProperty SearchModeProperty =
            DependencyProperty.Register(
                "SearchMode",
                typeof(SearchMode),
                typeof(SearchTextBox),
                new PropertyMetadata(SearchMode.Instant));

        private static DependencyProperty HasTextProperty =
            DependencyProperty.Register(
                "HasText",
                typeof(bool),
                typeof(SearchTextBox),
                new PropertyMetadata(false));
        #endregion

        public SearchTextBox()
        {
            this.DefaultStyleKey = typeof(SearchTextBox);

            base.TextChanged +=SearchTextBox_TextChanged;
        }

        void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
 	        HasText = Text.Length != 0;
        }

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public Brush LabelTextColor
        {
            get { return (Brush)GetValue(LabelTextColorProperty); }
            set { SetValue(LabelTextColorProperty, value); }
        }

        public SearchMode SearchMode
        {
            get { return (SearchMode)GetValue(SearchModeProperty); }
            set { SetValue(SearchModeProperty, value); }
        }

        public bool HasText
        {
            get { return (bool)GetValue(HasTextProperty); }
            private set { SetValue(HasTextProperty, value); }
        }
    }
}
