using System.Windows;
using System.Windows.Controls;
using Azuria.Main.Search;

namespace Azuria.Example.Controls.Search
{
    public class SortByRadionButton : RadioButton
    {
        public static readonly DependencyProperty SortByProperty =
            DependencyProperty.Register(nameof(SortBy), typeof (SearchHelper.SortAnimeManga)
                , typeof (SortByRadionButton),
                new FrameworkPropertyMetadata(SearchHelper.SortAnimeManga.Relevance, SortByPropertyChanged));

        public SortByRadionButton()
        {
            this.Content = this.SortBy;
        }

        #region Properties

        public SearchHelper.SortAnimeManga SortBy
        {
            get { return (SearchHelper.SortAnimeManga) this.GetValue(SortByProperty); }
            set { this.SetValue(SortByProperty, value); }
        }

        #endregion

        #region

        private static void SortByPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SortByRadionButton lRadioButton = d as SortByRadionButton;
            if (lRadioButton != null && lRadioButton.Content is SearchHelper.SortAnimeManga)
            {
                lRadioButton.Content = e.NewValue;
            }
        }

        #endregion
    }
}