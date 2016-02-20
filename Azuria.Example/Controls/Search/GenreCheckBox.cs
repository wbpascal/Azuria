using System.Windows;
using System.Windows.Controls;
using Azuria.Main.Minor;

namespace Azuria.Example.Controls.Search
{
    public class GenreCheckBox : CheckBox
    {
        public static readonly DependencyProperty GenreProperty =
            DependencyProperty.Register(nameof(Genre), typeof (GenreObject.GenreType)
                , typeof (GenreCheckBox),
                new FrameworkPropertyMetadata(GenreObject.GenreType.Adventure, GenrePropertyChanged));

        public GenreCheckBox()
        {
            this.Content = this.Genre;
        }

        #region Properties

        public GenreObject.GenreType Genre
        {
            get { return (GenreObject.GenreType) this.GetValue(GenreProperty); }
            set { this.SetValue(GenreProperty, value); }
        }

        #endregion

        #region

        private static void GenrePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GenreCheckBox lCheckBox = d as GenreCheckBox;
            if (lCheckBox != null && lCheckBox.Content is GenreObject.GenreType)
            {
                lCheckBox.Content = e.NewValue;
            }
        }

        #endregion
    }
}