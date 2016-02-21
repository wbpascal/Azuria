using System.Windows;
using System.Windows.Controls;
using Azuria.Main.Search;

namespace Azuria.Example.Controls.Search
{
    public class AnimeMangaTypeRadioButton : RadioButton
    {
        public static readonly DependencyProperty AnimeMangaTypeProperty =
            DependencyProperty.Register(nameof(AnimeMangaType), typeof (SearchHelper.AnimeMangaType)
                , typeof (AnimeMangaTypeRadioButton),
                new FrameworkPropertyMetadata(SearchHelper.AnimeMangaType.All, AnimeMangaTypePropertyChanged));

        public AnimeMangaTypeRadioButton()
        {
            this.Content = this.AnimeMangaType;
        }

        #region Properties

        public SearchHelper.AnimeMangaType AnimeMangaType
        {
            get { return (SearchHelper.AnimeMangaType) this.GetValue(AnimeMangaTypeProperty); }
            set { this.SetValue(AnimeMangaTypeProperty, value); }
        }

        #endregion

        #region

        private static void AnimeMangaTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AnimeMangaTypeRadioButton lRadioButton = d as AnimeMangaTypeRadioButton;
            if (lRadioButton != null && lRadioButton.Content is SearchHelper.AnimeMangaType)
            {
                lRadioButton.Content = e.NewValue;
            }
        }

        #endregion
    }
}