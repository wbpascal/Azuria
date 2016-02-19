using Azuria.Main.Search;
using System.Windows;
using System.Windows.Controls;

namespace Azuria.Example.Controls.Search
{
    public class SortByRadionButton : RadioButton
    {
        public static readonly DependencyProperty SortByProperty =
                DependencyProperty.Register(nameof(SortBy), typeof(SearchHelper.SortAnimeManga)
                , typeof(SortByRadionButton), new FrameworkPropertyMetadata(SearchHelper.SortAnimeManga.Relevanz, SortByPropertyChanged));

        private static void SortByPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SortByRadionButton lRadioButton = d as SortByRadionButton;
            if (lRadioButton != null && lRadioButton.Content is SearchHelper.SortAnimeManga)
            {
                lRadioButton.Content = e.NewValue;
            }
        }

        public SortByRadionButton()
        {
            this.Content = this.SortBy;
        }

        public SearchHelper.SortAnimeManga SortBy
        {
            get { return (SearchHelper.SortAnimeManga)this.GetValue(SortByProperty); }
            set
            {
                this.SetValue(SortByProperty, value);
            }
        }
    }
}
